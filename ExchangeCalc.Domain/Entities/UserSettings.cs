using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeCalc.Domain.Entities
{
	/// <summary>
	/// Representa las configuraciones personalizadas de un usuario dentro del sistema.
	/// </summary>
	public class UserSettings
	{
		/// <summary>
		/// Identificador único de las configuraciones del usuario.
		/// </summary>
		public int Id { get; set; }
		/// <summary>
		/// Identificador del usuario al cual pertenecen estas configuraciones.
		/// </summary>
		public string UserId { get; set; } = "default-user";
		/// <summary>
		/// Moneda principal definida por el usuario (ejemplo: "EUR").
		/// </summary>
		public string PrimaryCurrency { get; set; } = "MXN";
	}
}
