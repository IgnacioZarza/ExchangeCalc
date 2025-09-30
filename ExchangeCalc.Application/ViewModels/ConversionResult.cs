using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeCalc.Application.ViewModels
{
	public class ConversionResult
	{
		/// <summary>
		/// Representa el resultado de una conversión de divisa.
		/// </summary>
		public string Currency { get; set; } = string.Empty;
		public decimal Rate { get; set; }
		public decimal ConvertedAmount { get; set; }
	}
}
