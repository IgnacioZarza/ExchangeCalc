using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeCalc.Application.Extensions
{
	public static class CurrencyExtensions
	{
		/// <summary>
		/// Convierte un decimal a un string con formato de moneda y el código de divisa.
		/// Ejemplo: 1234.56m.ToCurrency("USD") -> "1,234.56 USD"
		/// </summary>
		public static string ToCurrency(this decimal amount, string currencyCode, string culture = "en-US")
		{
			var cultureInfo = new System.Globalization.CultureInfo(culture);
			return string.Format(cultureInfo, "{0:N2} {1}", amount, currencyCode.ToUpper());
		}
	}
}
