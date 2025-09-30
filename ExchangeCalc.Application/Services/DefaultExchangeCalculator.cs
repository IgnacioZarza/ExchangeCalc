using ExchangeCalc.Application.Interfaces;
using ExchangeCalc.Domain.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeCalc.Application.Services
{
	/// <summary>
	/// Implementación por defecto del calculador de divisas,
	/// basada en el cliente de la API de tasas de cambio.
	/// </summary>
	public class DefaultExchangeCalculator : ExchangeCalculatorBase
	{
		private readonly IFxRatesApiClient _fxRatesApiClient;

		public DefaultExchangeCalculator(string baseCurrency, IEnumerable<string> targetCurrencies, IFxRatesApiClient fxRatesApiClient)
			: base(baseCurrency, targetCurrencies)
		{
			_fxRatesApiClient = fxRatesApiClient;
		}

		public override async Task<IDictionary<string, decimal>> CalculateAsync(decimal amount, DateTime? date = null)
		{
			IDictionary<string, decimal> rates = date.HasValue
			? await _fxRatesApiClient.GetRatesByDateAsync(date.Value, BaseCurrency, TargetCurrencies)
			: await _fxRatesApiClient.GetLatestRatesAsync(BaseCurrency, TargetCurrencies);

			return rates.ToDictionary(
				kvp => kvp.Key,
				kvp => kvp.Value * amount
			);
		}
	}
}
