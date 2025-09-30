using ExchangeCalc.Application.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeCalc.Application.ViewModels
{
	/// <summary>
	/// Modelo de vista para mostrar la información principal del panel de control
	/// de conversiones de divisas.
	/// </summary>
	public class DashboardViewModel
	{
		/// <summary>
		/// Divisa base seleccionada para las conversiones (por defecto "MXN").
		/// </summary>
		public string BaseCurrency { get; set; } = "MXN";
		/// <summary>
		/// Fecha de la consulta de los tipos de cambio (por defecto la fecha actual en UTC).
		/// </summary>
		public DateTime Date { get; set; } = DateTime.UtcNow.Date;
		/// <summary>
		/// Monto base que se convertirá a otras divisas.
		/// </summary>
		public decimal Amount { get; set; } = 1m;
		/// <summary>
		/// Resultados de la conversión hacia las divisas destino.
		/// </summary>
		public List<ConversionResult> Results { get; set; } = new();
		/// <summary>
		/// Diccionario con las divisas disponibles. 
		/// La clave es el código (ej. "USD") y el valor es la descripción (ej. "US Dollar").
		/// </summary>
		public IDictionary<string, string>? AvailableCurrencies { get; set; }
		/// <summary>
		/// Lista de divisas favoritas del usuario.
		/// </summary>
		public List<string> FavoriteCurrencies { get; set; } = new();
		/// <summary>
		/// Monto base formateado como divisa (ej. "1.00 EUR").
		/// </summary>
		public string AmountFormatted => Amount.ToCurrency(BaseCurrency);
	}
}
