using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeCalc.Domain.Entities
{
	/// <summary>
	/// Representa un registro de auditoría de las operaciones realizadas en la API de Exchange.
	/// Permite almacenar información para trazabilidad y debugging.
	/// </summary>
	public class ExchangeLog
	{
		/// <summary>
		/// Identificador único del log.
		/// </summary>
		public int Id { get; set; }
		/// <summary>
		/// Fecha y hora (en UTC) en que se generó el log.
		/// </summary>
		public DateTime Timestamp { get; set; } = DateTime.UtcNow;
		/// <summary>
		/// Ruta del endpoint invocado (ejemplo: "/api/exchange/convert").
		/// </summary>
		public string Route { get; set; } = string.Empty;
		/// <summary>
		/// Método HTTP utilizado (GET, POST, PUT, DELETE).
		/// </summary>
		public string HttpMethod { get; set; } = string.Empty;
		/// <summary>
		/// Parámetros enviados en la URL (query string).
		/// </summary>
		public string QueryString { get; set; } = string.Empty;
		/// <summary>
		/// Cuerpo de la petición (payload enviado en el request).
		/// </summary>
		public string Body { get; set; } = string.Empty;
		/// <summary>
		/// Dirección IP del cliente que realizó la petición.
		/// </summary>
		public string UserIp { get; set; } = string.Empty;
	}
}
