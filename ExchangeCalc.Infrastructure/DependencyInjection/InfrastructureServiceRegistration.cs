using ExchangeCalc.Application.Interfaces;
using ExchangeCalc.Application.Settings;
using ExchangeCalc.Domain.Interfaces;
using ExchangeCalc.Infrastructure.Persistence;
using ExchangeCalc.Infrastructure.Repositories;
using ExchangeCalc.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeCalc.Infrastructure.DependencyInjection
{
	/// <summary>
	/// Clase estática responsable de registrar los servicios de la capa de infraestructura
	/// en el contenedor de dependencias de .NET.
	/// 
	/// Expone métodos de extensión para <see cref="IServiceCollection"/> que facilitan
	/// la configuración de la base de datos, repositorios y clientes externos.
	/// </summary>
	public static class InfrastructureServiceRegistration
	{
		/// <summary>
		/// Método de extensión que registra todos los servicios de infraestructura necesarios
		/// para la aplicación, incluyendo el DbContext, el patrón Unit of Work y clientes HTTP externos.
		/// </summary>
		/// <param name="services">
		/// Contenedor de servicios de .NET Core donde se registrarán las dependencias.
		/// </param>
		/// <param name="configuration">
		/// Configuración de la aplicación (usualmente <c>appsettings.json</c>) que proporciona cadenas de conexión y parámetros.
		/// </param>
		/// <returns>
		/// La misma colección de servicios (<see cref="IServiceCollection"/>), lo que permite encadenar configuraciones.
		/// </returns>
		public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
		{
			// Registro del DbContext utilizando SQL Server como proveedor.
			// La cadena de conexión se obtiene desde la configuración (appsettings.json).
			services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

			// Registro del patrón Unit of Work con ciclo de vida Scoped.
			// Esto asegura que cada request HTTP tenga su propia instancia.
			services.AddScoped<IUnitOfWork, UnitOfWork>();

			// Registro de un cliente HTTP tipado para consumir el API de tipos de cambio (Frankfurter).
			// Se configura la dirección base y un timeout de 30 segundos.
			services.Configure<FxRatesApiSettings>(configuration.GetSection("FxRatesApi"));

			services.AddHttpClient<IFxRatesApiClient, FxRatesApiClient>((sp, c) =>
			{
				var options = sp.GetRequiredService<
					Microsoft.Extensions.Options.IOptions<FxRatesApiSettings>>().Value;

				c.BaseAddress = new Uri(options.BaseUrl);
				c.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);
			});

			return services;
		}
	}
}
