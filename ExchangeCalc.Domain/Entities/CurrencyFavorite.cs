using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeCalc.Domain.Entities
{
	/// <summary>
	/// Representa una moneda marcada como favorita por un usuario.
	/// Esta entidad se asocia directamente a un <see cref="UserId"/> y un código de moneda ISO.
	/// </summary>
	public class CurrencyFavorite
	{
		/// <summary>
		/// Identificador único de la entidad en la base de datos.
		/// </summary>
		public int Id { get; set; }
		/// <summary>
		/// Identificador del usuario que ha marcado la moneda como favorita.
		/// Puede estar vinculado a un sistema de autenticación externo.
		/// </summary>
		public string UserId { get; set; } = "default-user";
		/// <summary>
		/// Código ISO de la moneda (ejemplo: "USD", "EUR").
		/// </summary>
		public string CurrencyCode { get; set; } = string.Empty;
	}
}
