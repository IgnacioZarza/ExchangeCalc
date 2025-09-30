using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeCalc.Domain.Entities
{
	/// <summary>
	/// Representa un tipo de cambio (FX Rate) entre dos monedas en un momento específico.
	/// </summary>
	public class FxRate
	{
		/// <summary>
		/// Identificador único del tipo de cambio en la base de datos.
		/// </summary>
		public int Id { get; set; }
		/// <summary>
		/// Moneda base (ejemplo: "USD").
		/// </summary>
		public string BaseCurrency { get; set; } = string.Empty;
		/// <summary>
		/// Moneda objetivo (ejemplo: "EUR").
		/// </summary>
		public string TargetCurrency { get; set; } = string.Empty;
		/// <summary>
		/// Tasa de conversión de la <see cref="BaseCurrency"/> hacia la <see cref="TargetCurrency"/>.
		/// </summary>
		public decimal Rate { get; set; }
		/// <summary>
		/// Fecha y hora (en UTC) en que se obtuvo este tipo de cambio.
		/// </summary>
		public DateTime RetrievedAt { get; set; } = DateTime.UtcNow;
	}
}
