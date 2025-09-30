using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeCalc.Domain.Interfaces
{
	/// <summary>
	/// Contrato genérico para un repositorio de acceso a datos.
	/// Define operaciones básicas de persistencia y consulta
	/// aplicables a cualquier entidad del dominio.
	/// </summary>
	/// <typeparam name="T">
	/// Tipo de entidad de dominio gestionada por el repositorio.
	/// Debe ser una clase (<see cref="class"/>).
	/// </typeparam>
	public interface IGenericRepository<T> where T : class
	{
		/// <summary>
		/// Obtiene una entidad por su identificador único.
		/// </summary>
		/// <param name="id">Identificador de la entidad.</param>
		/// <returns>
		/// Una instancia de <typeparamref name="T"/> si existe; 
		/// de lo contrario, <c>null</c>.
		/// </returns>
		Task<T?> GetByIdAsync(int id);
		/// <summary>
		/// Obtiene todas las entidades de tipo <typeparamref name="T"/>.
		/// </summary>
		/// <returns>
		/// Una colección enumerable con todas las entidades persistidas.
		/// </returns>
		Task<IEnumerable<T>> GetAllAsync();
		/// <summary>
		/// Busca entidades que cumplan con una condición dada.
		/// </summary>
		/// <param name="predicate">
		/// Expresión lambda que representa el criterio de filtrado.
		/// </param>
		/// <returns>
		Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
		/// <summary>
		/// Agrega una nueva entidad al contexto de persistencia.
		/// </summary>
		/// <param name="entity">Entidad a agregar.</param>
		Task AddAsync(T entity);
		// <summary>
		/// Actualiza una entidad existente en el contexto de persistencia.
		/// </summary>
		/// <param name="entity">Entidad a actualizar.</param>
		void Update(T entity);
		/// <summary>
		/// Elimina una entidad del contexto de persistencia.
		/// </summary>
		/// <param name="entity">Entidad a eliminar.</param>
		void Remove(T entity);
	}
}
