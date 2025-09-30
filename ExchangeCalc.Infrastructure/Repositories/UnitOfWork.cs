using ExchangeCalc.Domain.Entities;
using ExchangeCalc.Domain.Interfaces;
using ExchangeCalc.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeCalc.Infrastructure.Repositories
{
	/// <summary>
	/// Implementación del patrón Unit of Work que coordina múltiples repositorios
	/// bajo un único contexto de base de datos, asegurando transacciones consistentes.
	/// </summary>
	public class UnitOfWork : IUnitOfWork
	{
		private readonly ApplicationDbContext _context;
		private IGenericRepository<CurrencyFavorite>? _currencyFavorites;
		private IGenericRepository<UserSettings>? _userSettings;
		private IGenericRepository<ExchangeLog>? _exchangeLogs;
		private IGenericRepository<FxRate>? _fxRates;

		/// <summary>
		/// Inicializa una nueva instancia de <see cref="UnitOfWork"/>.
		/// </summary>
		/// <param name="context">Contexto de base de datos inyectado.</param>
		public UnitOfWork(ApplicationDbContext context)
		{
			_context = context;
		}

		// Repositorios
		/// <inheritdoc/>
		public IGenericRepository<CurrencyFavorite> CurrencyFavorites
			=> _currencyFavorites ??= new GenericRepository<CurrencyFavorite>(_context) as IGenericRepository<CurrencyFavorite>;
		/// <inheritdoc/>
		public IGenericRepository<UserSettings> UserSettings
			=> _userSettings ??= new GenericRepository<UserSettings>(_context) as IGenericRepository<UserSettings>;
		/// <inheritdoc/>
		public IGenericRepository<ExchangeLog> ExchangeLogs
			=> _exchangeLogs ??= new GenericRepository<ExchangeLog>(_context) as IGenericRepository<ExchangeLog>;
		/// <inheritdoc/>
		public IGenericRepository<FxRate> FxRates
			=> _fxRates ??= new GenericRepository<FxRate>(_context) as IGenericRepository<FxRate>;

		// Métodos de persistencia
		/// <summary>
		/// Guarda los cambios pendientes en el contexto de manera asíncrona.
		/// </summary>
		/// <returns>Número de entidades afectadas.</returns>
		public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();
		/// <summary>
		/// Completa la transacción actual confirmando todos los cambios.
		/// 
		/// Nota: en esta implementación equivale a <see cref="SaveChangesAsync"/>,
		/// pero se expone para mayor claridad semántica.
		/// </summary>
		/// <returns>Número de entidades afectadas.</returns>
		public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

		// Liberar recursos
		/// <summary>
		/// Libera los recursos asociados al contexto de base de datos.
		/// </summary>
		public void Dispose() => _context.Dispose();
	}
}
