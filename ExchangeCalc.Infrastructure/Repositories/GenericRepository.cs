using ExchangeCalc.Domain.Interfaces;
using ExchangeCalc.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeCalc.Infrastructure.Repositories
{
	/// <summary>
	/// Implementación genérica del patrón Repository utilizando Entity Framework Core.
	/// Proporciona operaciones CRUD básicas para cualquier entidad del dominio.
	/// </summary>
	/// <typeparam name="T">Entidad de dominio gestionada por el repositorio.</typeparam>
	public class GenericRepository<T> : IGenericRepository<T> where T : class
	{
		/// <summary>
		/// Contexto de base de datos de EF Core.
		/// </summary>
		protected readonly ApplicationDbContext _context;
		/// <summary>
		/// Conjunto de entidades (<see cref="DbSet{TEntity}"/>) correspondiente al tipo <typeparamref name="T"/>.
		/// </summary>
		private readonly DbSet<T> _dbSet;

		/// <summary>
		/// Inicializa una nueva instancia del repositorio genérico.
		/// </summary>
		/// <param name="context">Contexto de base de datos inyectado.</param>
		public GenericRepository(ApplicationDbContext context)
		{
			_context = context;
			_dbSet = context.Set<T>();
		}

		/// <inheritdoc/>
		public async Task<T?> GetByIdAsync(int id) => await _dbSet.FindAsync(id);
		/// <inheritdoc/>
		public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();
		/// <inheritdoc/>
		public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
			=> await _dbSet.Where(predicate).ToListAsync();
		/// <inheritdoc/>
		public async Task AddAsync(T entity) => await _dbSet.AddAsync(entity);
		/// <inheritdoc/>
		public void Update(T entity) => _dbSet.Update(entity);
		/// <inheritdoc/>
		public void Remove(T entity) => _dbSet.Remove(entity);
	}
}
