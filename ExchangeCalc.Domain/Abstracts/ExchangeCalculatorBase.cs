using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeCalc.Domain.Abstracts
{
	/// <summary>
	/// Clase base abstracta que define el contrato para un calculador de divisas.
	/// Proporciona la lógica común para establecer una divisa base y un conjunto de divisas objetivo,
	/// y expone un método para calcular conversiones.
	/// </summary>
	public abstract class ExchangeCalculatorBase
	{
		/// <summary>
		/// Código de la divisa base (ejemplo: "EUR", "USD") desde la cual se harán las conversiones.
		/// </summary>
		public string BaseCurrency { get; }
		/// <summary>
		/// Colección de divisas destino a las que se convertirá la divisa base.
		/// </summary>
		public IEnumerable<string> TargetCurrencies { get; }
		/// <summary>
		/// Inicializa una nueva instancia de la clase <see cref="ExchangeCalculatorBase"/>.
		/// </summary>
		/// <param name="baseCurrency">Código de la divisa base usada en las conversiones.</param>
		/// <param name="targetCurrencies">Lista de códigos de divisas destino.</param>
		protected ExchangeCalculatorBase(string baseCurrency, IEnumerable<string> targetCurrencies)
		{
			BaseCurrency = baseCurrency;
			TargetCurrencies = targetCurrencies;
		}
		/// Calcula las tasas de conversión para un monto específico, opcionalmente en una fecha determinada.
		/// </summary>
		/// <param name="amount">Monto en la divisa base que será convertido.</param>
		/// <param name="date">
		/// Fecha de referencia para la tasa de cambio. 
		/// Si es <c>null</c>, se utiliza la tasa más reciente disponible.
		/// </param>
		/// <returns>
		/// Un diccionario donde la clave es el código de la divisa destino 
		/// y el valor es el monto convertido.
		/// </returns>
		public abstract Task<IDictionary<string, decimal>> CalculateAsync(decimal amount, DateTime? date = null);
	}
}
