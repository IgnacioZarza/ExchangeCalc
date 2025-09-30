using ExchangeCalc.Application.Interfaces;
using ExchangeCalc.Application.ViewModels;
using ExchangeCalc.Domain.Entities;
using ExchangeCalc.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ExchangeCalc.Application.Services
{
	/// <summary>
	/// Define las operaciones relacionadas con divisas, favoritos del usuario 
	/// y configuración de moneda principal.
	/// </summary>
	public interface IExchangeService
	{
		/// <summary>
		/// Obtiene las monedas disponibles desde la API externa.
		/// </summary>
		Task<IDictionary<string, string>> GetAvailableCurrenciesAsync();
		/// <summary>
		/// Agrega una divisa a la lista de favoritos de un usuario.
		/// </summary>
		Task AddFavoriteAsync(string userId, string currency);
		/// <summary>
		/// Elimina una divisa de la lista de favoritos de un usuario.
		/// </summary>
		Task RemoveFavoriteAsync(string userId, string currency);
		/// <summary>
		/// Establece la divisa principal en la configuración de un usuario.
		/// </summary>
		Task SetPrimaryCurrencyAsync(string userId, string currency);
		/// <summary>
		/// Genera el dashboard de conversiones para un usuario,
		/// en base a su moneda principal y sus divisas favoritas.
		/// </summary>
		Task<DashboardViewModel> GetDashboardAsync(string userId, DateTime date, decimal amount);
	}

	/// <summary>
	/// Servicio que implementa la lógica de negocio para divisas, 
	/// utilizando una API externa de tipos de cambio y persistencia con UnitOfWork.
	/// </summary>
	public class ExchangeService : IExchangeService
	{
		private readonly IFxRatesApiClient _fxApi;
		private readonly IUnitOfWork _uow;
		private readonly ILogger<ExchangeService> _logger;

		/// <summary>
		/// Constructor del servicio de divisas.
		/// </summary>
		public ExchangeService(IFxRatesApiClient fxApi, IUnitOfWork uow, ILogger<ExchangeService> logger)
		{
			_fxApi = fxApi;
			_uow = uow;
			_logger = logger;
		}

		/// <inheritdoc/>
		public async Task<IDictionary<string, string>> GetAvailableCurrenciesAsync()
		{
			try
			{
				return await _fxApi.GetAvailableCurrenciesAsync();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error obteniendo las monedas disponibles desde la API externa.");
				throw;
			}
		}

		/// <inheritdoc/>
		public async Task AddFavoriteAsync(string userId, string currency)
		{
			try
			{
				var exists = (await _uow.CurrencyFavorites
					.FindAsync(cf => cf.UserId == userId && cf.CurrencyCode == currency))
					.Any();

				if (!exists)
				{
					await _uow.CurrencyFavorites.AddAsync(new CurrencyFavorite
					{
						UserId = userId,
						CurrencyCode = currency
					});
					await _uow.SaveChangesAsync();
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error agregando la moneda {Currency} a favoritos para el usuario {UserId}.", currency, userId);
				throw;
			}
		}

		/// <inheritdoc/>
		public async Task RemoveFavoriteAsync(string userId, string currency)
		{
			try
			{
				var items = await _uow.CurrencyFavorites
					.FindAsync(cf => cf.UserId == userId && cf.CurrencyCode == currency);

				var toRemove = items.FirstOrDefault();
				if (toRemove != null)
				{
					_uow.CurrencyFavorites.Remove(toRemove);
					await _uow.SaveChangesAsync();
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error eliminando la moneda {Currency} de favoritos para el usuario {UserId}.", currency, userId);
				throw;
			}
		}

		/// <inheritdoc/>
		public async Task SetPrimaryCurrencyAsync(string userId, string currency)
		{
			try
			{
				var setting = (await _uow.UserSettings.FindAsync(s => s.UserId == userId)).FirstOrDefault();
				if (setting == null)
				{
					await _uow.UserSettings.AddAsync(new UserSettings { UserId = userId, PrimaryCurrency = currency });
				}
				else
				{
					setting.PrimaryCurrency = currency;
					_uow.UserSettings.Update(setting);
				}
				await _uow.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error configurando la moneda principal {Currency} para el usuario {UserId}.", currency, userId);
				throw;
			}
		}
		/// <inheritdoc/>
		public async Task<DashboardViewModel> GetDashboardAsync(string userId, DateTime date, decimal amount)
		{
			try
			{
				var favorites = (await _uow.CurrencyFavorites
					.FindAsync(cf => cf.UserId == userId))
					.Select(f => f.CurrencyCode)
					.ToList();

				var setting = (await _uow.UserSettings.FindAsync(s => s.UserId == userId))
					.FirstOrDefault();
				var baseCurrency = setting?.PrimaryCurrency ?? "EUR";

				if (!favorites.Contains(baseCurrency))
				{
					favorites.Add(baseCurrency);
				}

				if (!favorites.Any())
				{
					return new DashboardViewModel
					{
						BaseCurrency = baseCurrency,
						Date = date,
						Amount = amount,
						Results = new List<ConversionResult>
				{
					new ConversionResult
					{
						Currency = baseCurrency,
						Rate = 1m,
						ConvertedAmount = amount
					}
				},
						FavoriteCurrencies = favorites
					};
				}

				var calculator = new DefaultExchangeCalculator(baseCurrency, favorites, _fxApi);
				var conversions = await calculator.CalculateAsync(amount, date);

				foreach (var kvp in conversions)
				{
					var fxRate = new FxRate
					{
						BaseCurrency = baseCurrency,
						TargetCurrency = kvp.Key,
						Rate = kvp.Value / amount,
						RetrievedAt = date
					};
					await _uow.FxRates.AddAsync(fxRate);
				}
				await _uow.SaveChangesAsync();

				return new DashboardViewModel
				{
					BaseCurrency = baseCurrency,
					Date = date,
					Amount = amount,
					Results = conversions.Select(kvp => new ConversionResult
					{
						Currency = kvp.Key,
						Rate = kvp.Value / amount,
						ConvertedAmount = Math.Round(kvp.Value, 4)
					}).ToList(),
					FavoriteCurrencies = favorites
				};
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error generando el dashboard de divisas para el usuario {UserId}.", userId);
				throw;
			}
		}
	}
}
