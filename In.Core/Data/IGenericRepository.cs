using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace In.Core.Data
{
	public interface IGenericRepository<T> where T : class
	{
		T Add(T t);
		ValueTask<EntityEntry<T>> AddAsync(T t);
		void AddRange(IEnumerable<T> entities);
		void AddRange(params T[] entities);
		Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
		Task AddRangeAsync(params T[] entities);
		int Count();
		Task<int> CountAsync();
		void Delete(T entity);
		Task<int> DeleteAsync(T entity);
		void Dispose();
		T Find(Expression<Func<T, bool>> match);
		ICollection<T> FindAll(Expression<Func<T, bool>> match);
		Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match);
		Task<T> FindAsync(Expression<Func<T, bool>> match, params Expression<Func<T, object>>[] includeProperties);
		IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
		Task<ICollection<T>> FindByAsync(Expression<Func<T, bool>> predicate);
		T Get(object id);
		IQueryable<T> GetAll();
		Task<ICollection<T>> GetAllAsync();
		Task<ICollection<T>> GetAllAsyncIncluding(params Expression<Func<T, object>>[] includeProperties);
		IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties);
		Task<T> GetAsync(object id);
		void Save();
		void SaveChangesAsync();
		Task<int> SaveAsync();
		T Update(T t, object key);
		Task<T> UpdateAsync(T t, object key);
		T First();
		T First(Expression<Func<T, bool>> predicate);
		T FirstOrDefault();
		T FirstOrDefault(Expression<Func<T, bool>> predicate);
		Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
		bool Any();
		bool Any(Expression<Func<T, bool>> predicate);
	}
}