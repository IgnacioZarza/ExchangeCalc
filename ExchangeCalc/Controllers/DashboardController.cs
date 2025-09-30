using ExchangeCalc.Application.Services;
using ExchangeCalc.Domain.Interfaces;
using ExchangeCalc.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeCalc.Controllers
{
	/// <summary>
	/// Controlador MVC que administra la vista del Dashboard.
	/// Permite consultar tipos de cambio, configurar moneda principal
	/// y gestionar monedas favoritas de un usuario.
	/// </summary>
	public class DashboardController : Controller
	{
		private readonly IExchangeService _exchangeService;
		private readonly IUnitOfWork _uow;
		private readonly ILogger<ExchangeService> _logger;

		/// <summary>
		/// Inicializa una nueva instancia de <see cref="DashboardController"/>.
		/// </summary>
		/// <param name="exchangeService">Servicio de lógica de negocio para conversiones y configuración.</param>
		/// <param name="uow">Unidad de trabajo para persistencia de datos.</param>
		public DashboardController(IExchangeService exchangeService, IUnitOfWork uow, ILogger<ExchangeService> logger)
		{
			_exchangeService = exchangeService;
			_uow = uow;
			_logger = logger;
		}

		/// <summary>
		/// Muestra la vista principal del Dashboard con las tasas de cambio.
		/// </summary>
		/// <param name="date">Fecha de la consulta (opcional, por defecto hoy).</param>
		/// <param name="amount">Monto base a convertir (opcional, por defecto 1).</param>
		public async Task<IActionResult> Index(DateTime? date, decimal? amount)
		{
			try
			{
				var userId = "default-user";
				var d = date ?? DateTime.UtcNow.Date;
				var a = amount ?? 1m;

				var vm = await _exchangeService.GetDashboardAsync(userId, d, a);
				vm.AvailableCurrencies = (await _exchangeService.GetAvailableCurrenciesAsync()) as IDictionary<string, string>;

				return View(vm);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Ocurrió un error al cargar el Dashboard.");
				return View("Error", $"Ocurrió un error al cargar el Dashboard: {ex.Message}");
			}
		}

		/// <summary>
		/// Agrega una moneda a la lista de favoritas del usuario.
		/// </summary>
		/// <param name="currency">Código de la moneda a agregar.</param>
		[HttpPost]
		public async Task<IActionResult> AddFavorite(string currency)
		{
			try
			{
				var userId = "default-user";
				if (!string.IsNullOrEmpty(currency))
					await _exchangeService.AddFavoriteAsync(userId, currency);

				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error al agregar favorito.");
				return BadRequest($"Error al agregar favorito: {ex.Message}");
			}
		}

		/// <summary>
		/// Elimina una moneda de la lista de favoritas del usuario.
		/// </summary>
		/// <param name="currency">Código de la moneda a eliminar.</param>
		[HttpPost]
		public async Task<IActionResult> RemoveFavorite(string currency)
		{
			try
			{
				var userId = "default-user";
				if (!string.IsNullOrEmpty(currency))
					await _exchangeService.RemoveFavoriteAsync(userId, currency);

				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error al eliminar favorito.");
				return BadRequest($"Error al eliminar favorito: {ex.Message}");
			}
		}

		/// <summary>
		/// Define la moneda principal del usuario para conversiones.
		/// </summary>
		/// <param name="currency">Código de la moneda principal.</param>
		[HttpPost]
		public async Task<IActionResult> SetPrimary(string currency)
		{
			try
			{
				var userId = "default-user";
				if (!string.IsNullOrEmpty(currency))
					await _exchangeService.SetPrimaryCurrencyAsync(userId, currency);

				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error al establecer moneda principal.");
				return BadRequest($"Error al establecer moneda principal: {ex.Message}");
			}
		}
	}
}
