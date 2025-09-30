using System.Net;
using System.Text.Json;

namespace ExchangeCalc.Middlewares
{
	public class ErrorHandlingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ErrorHandlingMiddleware> _logger;

		public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error no controlado: {Message}", ex.Message);

				context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
				context.Response.ContentType = "application/json";

				var errorResponse = new
				{
					type = "https://httpstatuses.com/500",
					title = "Error interno del servidor",
					status = context.Response.StatusCode,
					detail = "Ha ocurrido un error inesperado. Contacte al administrador si persiste."
				};

				await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
			}
		}
	}
}
