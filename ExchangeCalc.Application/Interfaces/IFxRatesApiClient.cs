using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeCalc.Application.Interfaces
{
	/// <summary>
	/// Define las operaciones necesarias para comunicarse con la API externa de tipos de cambio.
	/// 
	/// Esta interfaz abstrae el cliente HTTP o cualquier otra implementación que obtenga 
	/// información de divisas, lo que facilita pruebas unitarias y permite cambiar de proveedor 
	/// sin afectar la lógica de negocio.
	/// </summary>
	public interface IFxRatesApiClient
	{
		/// <summary>
		/// Obtiene la lista de monedas disponibles desde la API externa.
		/// </summary>
		/// <returns>
		/// Un diccionario donde la clave es el código de la divisa (ej. "USD") y el valor es 
		/// la descripción (ej. "United States Dollar").
		/// </returns>
		Task<IDictionary<string, string>> GetAvailableCurrenciesAsync();
		/// <summary>
		/// Obtiene los tipos de cambio más recientes desde la API externa.
		/// </summary>
		/// <param name="baseCurrency">Código de la divisa base (ej. "EUR").</param>
		/// <param name="targets">Colección de divisas destino (ej. ["USD", "MXN"]).</param>
		/// <returns>
		/// Un diccionario donde la clave es el código de la divisa destino y el valor es el tipo de cambio.
		/// </returns>
		Task<IDictionary<string, decimal>> GetLatestRatesAsync(string baseCurrency, IEnumerable<string> targets);
		/// <summary>
		/// Obtiene los tipos de cambio para una fecha específica desde la API externa.
		/// </summary>
		/// <param name="date">Fecha para la cual se requiere el tipo de cambio.</param>
		/// <param name="baseCurrency">Código de la divisa base (ej. "EUR").</param>
		/// <param name="targets">Colección de divisas destino.</param>
		/// <returns>
		/// Un diccionario donde la clave es el código de la divisa destino y el valor es el tipo de cambio.
		/// </returns>
		Task<IDictionary<string, decimal>> GetRatesByDateAsync(DateTime date, string baseCurrency, IEnumerable<string> targets);
	}
}
