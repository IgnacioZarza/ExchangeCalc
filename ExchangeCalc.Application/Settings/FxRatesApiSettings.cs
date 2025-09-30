using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeCalc.Application.Settings
{
	/// <summary>
	/// Representa la configuración para el cliente externo de tasas de cambio (FX Rates API).
	/// Se enlaza con la sección <c>FxRatesApi</c> de appsettings.json.
	/// </summary>
	public class FxRatesApiSettings
	{
		/// <summary>
		/// URL base del servicio externo de tipos de cambio.
		/// </summary>
		public string BaseUrl { get; set; } = string.Empty;

		/// <summary>
		/// Tiempo máximo de espera para las peticiones HTTP (en segundos).
		/// Valor por defecto: 30 segundos.
		/// </summary>
		public int TimeoutSeconds { get; set; } = 30;
	}
}
