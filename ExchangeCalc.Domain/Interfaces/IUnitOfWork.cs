using ExchangeCalc.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeCalc.Domain.Interfaces
{
	/// <summary>
	/// Contrato para la unidad de trabajo (Unit of Work) que coordina múltiples repositorios.
	/// Permite trabajar con varias entidades bajo el mismo contexto transaccional
	/// y asegura la persistencia consistente de los cambios.
	/// </summary>
	public interface IUnitOfWork : IDisposable
	{
		/// <summary>
		/// Repositorio para gestionar monedas favoritas de los usuarios.
		/// </summary>
		IGenericRepository<CurrencyFavorite> CurrencyFavorites { get; }
		/// <summary>
		/// Repositorio para gestionar configuraciones personalizadas de usuarios.
		/// </summary>
		IGenericRepository<UserSettings> UserSettings { get; }
		/// <summary>
		/// Repositorio para gestionar los registros de auditoría de la aplicación.
		/// </summary>
		IGenericRepository<ExchangeLog> ExchangeLogs { get; }
		/// <summary>
		/// Repositorio para gestionar los tipos de cambio (FX Rates).
		/// </summary>
		IGenericRepository<FxRate> FxRates { get; }
		/// <summary>
		/// Guarda de manera asíncrona los cambios pendientes en el contexto de persistencia.
		/// </summary>
		/// <returns>
		/// /// El número de entidades afectadas en la base de datos.
		/// </returns>
		Task<int> SaveChangesAsync();
		/// <summary>
		/// Completa la transacción actual y confirma todos los cambios realizados.
		/// 
		/// Nota: En muchos casos, este método es equivalente a <see cref="SaveChangesAsync"/>,
		/// pero se expone para dar semántica adicional cuando se aplica el patrón Unit of Work.
		/// </summary>
		/// <returns>
		/// El número de entidades afectadas en la base de datos.
		/// </returns>
		Task<int> CompleteAsync();
	}
}
