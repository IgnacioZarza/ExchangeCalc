using ExchangeCalc.Application.Interfaces;
using ExchangeCalc.Application.Services;
using ExchangeCalc.Application.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace ExchangeCalc.Infrastructure.Services
{
	// <summary>
	/// Cliente HTTP que consume la API externa de tasas de cambio (Frankfurter API).
	/// Implementa el contrato <see cref="IFxRatesApiClient"/>.
	/// </summary>
	public class FxRatesApiClient : IFxRatesApiClient
	{
		private readonly HttpClient _http;
		private readonly string _baseUrl;
		private readonly ILogger<ExchangeService> _logger;

		/// <summary>
		/// Inicializa una nueva instancia de <see cref="FxRatesApiClient"/>.
		/// </summary>
		/// <param name="http">Instancia de <see cref="HttpClient"/> configurada en el contenedor de dependencias.</param>
		/// <param name="options">Opciones de configuración que contienen la URL base del API.</param>
		public FxRatesApiClient(HttpClient http, IOptions<FxRatesApiSettings> options, ILogger<ExchangeService> logger)
		{
			_http = http;
			_baseUrl = options.Value.BaseUrl;
			_logger = logger;
		}

		/// <inheritdoc/>
		public async Task<IDictionary<string, string>> GetAvailableCurrenciesAsync()
		{
			try
			{
				var resp = await _http.GetFromJsonAsync<Dictionary<string, string>>($"{_baseUrl}currencies");
				return resp ?? new Dictionary<string, string>();
			}
            catch (Exception ex)
            {
				_logger.LogError(ex, "Error al obtener las monedas disponibles.");
				return new Dictionary<string, string>();
			}
		}

		/// <inheritdoc/>
		public async Task<IDictionary<string, decimal>> GetLatestRatesAsync(string baseCurrency, IEnumerable<string> targets)
		{
			var cleanTargets = targets
				.Where(t => !string.IsNullOrWhiteSpace(t))
				.Select(t => t.Trim().ToUpperInvariant())
				.Distinct()
				.Where(t => !t.Equals(baseCurrency.Trim().ToUpperInvariant()))
				.ToList();

			if (!cleanTargets.Any())
				return new Dictionary<string, decimal>();

			var to = string.Join(',', cleanTargets);
			var url = $"{_baseUrl}latest?base={baseCurrency.ToUpperInvariant()}&symbols={to}";

			try
			{
				var response = await _http.GetFromJsonAsync<FxRatesResponse>(url);
				return response?.Rates ?? new Dictionary<string, decimal>();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error al obtener los tipos de cambio más recientes.");
				return new Dictionary<string, decimal>();
			}
		}

		/// <inheritdoc/>
		public async Task<IDictionary<string, decimal>> GetRatesByDateAsync(DateTime date, string baseCurrency, IEnumerable<string> targets)
		{
			var cleanTargets = targets
				.Where(t => !string.IsNullOrWhiteSpace(t))
				.Select(t => t.Trim().ToUpperInvariant())
				.Distinct()
				.Where(t => !t.Equals(baseCurrency.Trim().ToUpperInvariant()))
				.ToList();

			if (!cleanTargets.Any())
				return new Dictionary<string, decimal>();

			var to = string.Join(',', cleanTargets);
			var dateStr = date.ToString("yyyy-MM-dd");
			var url = $"{_baseUrl}{dateStr}?from={baseCurrency.ToUpperInvariant()}&to={to}";

			try
			{
				var response = await _http.GetFromJsonAsync<FxRatesResponse>(url);
				return response?.Rates ?? new Dictionary<string, decimal>();
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error al obtener la divisa {BaseCurrency} de la fecha {Date}.", baseCurrency, date);
				return new Dictionary<string, decimal>();
			}
		}
	}
}
