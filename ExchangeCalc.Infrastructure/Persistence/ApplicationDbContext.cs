using ExchangeCalc.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeCalc.Infrastructure.Persistence
{
	/// <summary>
	/// Contexto principal de Entity Framework Core para la aplicación ExchangeCalc.
	/// Administra el acceso a las entidades de dominio y configura las reglas de persistencia.
	/// </summary>
	public class ApplicationDbContext : DbContext
	{
		/// <summary>
		/// Constructor que recibe las opciones de configuración del contexto.
		/// </summary>
		/// <param name="options">
		/// Opciones de <see cref="DbContextOptions"/> configuradas en la capa de infraestructura.
		/// </param>
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
		/// <summary>
		/// Tabla de monedas favoritas de los usuarios.
		/// </summary>
		public DbSet<CurrencyFavorite> CurrencyFavorites { get; set; }
		/// <summary>
		/// Tabla de configuraciones personalizadas de usuarios.
		/// </summary>
		public DbSet<UserSettings> UserSettings { get; set; }
		/// <summary>
		/// Tabla de registros de auditoría y logs de operaciones.
		/// </summary>
		public DbSet<ExchangeLog> ExchangeLogs { get; set; }
		/// <summary>
		/// Tabla de tipos de cambio (FX Rates).
		/// </summary>
		public DbSet<FxRate> FxRates { get; set; }

		/// <summary>
		/// Método llamado al construir el modelo de EF Core.
		/// Aquí se definen restricciones, índices y configuraciones específicas de las entidades.
		/// </summary>
		/// <param name="modelBuilder">Constructor del modelo de EF Core.</param>
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// CurrencyFavorite: índice único compuesto para evitar duplicados de moneda por usuario
			modelBuilder.Entity<CurrencyFavorite>()
				.HasIndex(c => new { c.UserId, c.CurrencyCode })
				.IsUnique();

			// FxRate: precisión definida para la columna Rate (18 dígitos, 6 decimales)
			modelBuilder.Entity<FxRate>()
				.Property(f => f.Rate)
				.HasPrecision(18, 6);
		}
	}
}
