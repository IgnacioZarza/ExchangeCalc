using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeCalc.Infrastructure.Services
{
	/// <summary>
	/// Representa la respuesta JSON devuelta por la API de tasas de cambio (Frankfurter API).
	/// Contiene la moneda base, la fecha de la cotización y el diccionario de tasas.
	/// </summary>
	public class FxRatesResponse
	{
		/// <summary>
		/// Código de la moneda base sobre la cual se calculan las tasas de cambio.
		/// Ejemplo: "EUR".
		/// </summary>
		public string Base { get; set; } = string.Empty;
		/// <summary>
		/// Fecha de la cotización en formato "yyyy-MM-dd".
		/// Usada para identificar la vigencia de las tasas de cambio.
		/// </summary>
		public string Date { get; set; } = string.Empty;
		/// <summary>
		/// Diccionario de tasas de cambio, donde la clave es el código de la moneda destino
		/// (ejemplo: "USD") y el valor es la tasa correspondiente.
		/// </summary>
		public Dictionary<string, decimal> Rates { get; set; } = new();
	}
}
