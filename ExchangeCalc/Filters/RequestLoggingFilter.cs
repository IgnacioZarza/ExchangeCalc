using ExchangeCalc.Domain.Entities;
using ExchangeCalc.Domain.Interfaces;
using ExchangeCalc.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace ExchangeCalc.Filters
{
	/// <summary>
	/// Filtro de acción que registra información detallada de cada petición HTTP entrante.
	/// Los datos se almacenan en la tabla <see cref="ExchangeLog"/> para fines de auditoría y monitoreo.
	/// </summary>
	public class RequestLoggingFilter : IAsyncActionFilter
	{
		private readonly IUnitOfWork _uow;
		/// <summary>
		/// Inicializa una nueva instancia del filtro de logging de requests.
		/// </summary>
		/// <param name="uow">Unidad de trabajo para persistir los registros en la base de datos.</param>
		public RequestLoggingFilter(IUnitOfWork uow) => _uow = uow;

		/// <summary>
		/// Intercepta la ejecución de una acción en el pipeline MVC, captura la información
		/// de la petición HTTP (ruta, método, querystring, body, IP del usuario) y la persiste.
		/// </summary>
		/// <param name="context">Contexto de la acción en ejecución.</param>
		/// <param name="next">Delegado que representa la siguiente acción en el pipeline.</param>
		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			var request = context.HttpContext.Request;

			string body = string.Empty;
			if (request.ContentLength > 0 && request.Body.CanSeek)
			{
				request.Body.Position = 0;
				using var sr = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
				body = await sr.ReadToEndAsync();
				request.Body.Position = 0;
			}

			var log = new ExchangeLog
			{
				Timestamp = DateTime.UtcNow,
				Route = request.Path,
				HttpMethod = request.Method,
				QueryString = request.QueryString.HasValue ? request.QueryString.Value : string.Empty,
				Body = body,
				UserIp = context.HttpContext.Connection.RemoteIpAddress?.ToString() ?? string.Empty
			};

			await _uow.ExchangeLogs.AddAsync(log);
			await _uow.SaveChangesAsync();

			await next();
		}
	}
}
