using In.Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace In.Core.Data
{
	public class GenericRepository<T> : IGenericRepository<T> where T : Auditable
	{
		protected IHttpContextAccessor _httpContext;
		protected IdentityDbContext<ApplicationUser> _context;
		protected Guid User { get; }
		protected Dictionary<string, string> Roles { get; }

		public GenericRepository(IHttpContextAccessor httpContext, IdentityDbContext<ApplicationUser> context, Guid user)
		{
			_context = context;
			_httpContext = httpContext;
			User = user;
			Roles = new Dictionary<string, string>();
			_context.UserRoles.Where(r => r.UserId == User.ToString()).Select(r => new { Id = r.RoleId, _context.Roles.First(d => d.Id == r.RoleId).Name }).ToList()
				.ForEach(r => Roles.Add(r.Id, r.Name));
		}

		public bool IsInRole(string role)
		{
			return Roles.ContainsValue(role);
		}

		public virtual IQueryable<T> GetAll()
		{
			return ApplyUserFilter(_context.Set<T>());
		}

		public virtual async Task<ICollection<T>> GetAllAsync()
		{
			return await GetAll().ToListAsync().ConfigureAwait(false);
		}

		public virtual async Task<ICollection<T>> GetAllAsyncIncluding(params Expression<Func<T, object>>[] includeProperties)
		{
			IQueryable<T> queryable = GetAll();
			foreach (Expression<Func<T, object>> includeProperty in includeProperties)
			{
				queryable = queryable.Include(includeProperty);
			}

			return await queryable.ToListAsync().ConfigureAwait(false);
		}

		public virtual T Get(object id)
		{
			List<T> output = new() { _context.Set<T>().Find(id) };
			return ApplyUserFilter(output.AsQueryable()).First();
		}

		public virtual async Task<T> GetAsync(object id)
		{
			List<T> output = new() { _context.Set<T>().Find(id) };
			return await ApplyUserFilter(output.AsQueryable()).FirstAsync().ConfigureAwait(false);
		}

		public virtual void AddRange(IEnumerable<T> entities)
		{
			_context.AddRange(entities);
		}

		public virtual void AddRange(params T[] entities)
		{
			_context.AddRange(entities);
		}

		public virtual Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
		{
			return _context.AddRangeAsync(entities, cancellationToken);
		}

		public virtual Task AddRangeAsync(params T[] entities)
		{
			return _context.AddRangeAsync(entities);
		}

		public virtual T Add(T t)
		{
			_context.Set<T>().Add(t);
			return t;
		}

		public virtual ValueTask<EntityEntry<T>> AddAsync(T t)
		{
			return _context.Set<T>().AddAsync(t);
		}

		public virtual T Find(Expression<Func<T, bool>> match)
		{
			return GetAll().SingleOrDefault(match);
		}

		public virtual async Task<T> FindAsync(Expression<Func<T, bool>> match, params Expression<Func<T, object>>[] includeProperties)
		{
			IQueryable<T> queryable = GetAll();
			foreach (Expression<Func<T, object>> includeProperty in includeProperties)
			{
				queryable = queryable.Include(includeProperty);
			}

			return await queryable.FirstAsync(match).ConfigureAwait(false);
		}

		public virtual ICollection<T> FindAll(Expression<Func<T, bool>> match)
		{
			return GetAll().Where(match).ToList();
		}

		public virtual async Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> match)
		{
			return await GetAll().Where(match).ToListAsync().ConfigureAwait(false);
		}

		public virtual void Delete(T entity)
		{
			_context.Set<T>().Remove(entity);
		}

		public virtual async Task<int> DeleteAsync(T entity)
		{
			_context.Set<T>().Remove(entity);
			return await _context.SaveChangesAsync().ConfigureAwait(false);
		}

		public virtual T Update(T t, object key)
		{
			if (t == null)
			{
				return null;
			}

			T exist = Get(key);
			if (exist != null)
			{
				_context.Entry(exist).CurrentValues.SetValues(t);
			}
			return exist;
		}

		public virtual async Task<T> UpdateAsync(T t, object key)
		{
			if (t == null)
			{
				return null;
			}

			T exist = await GetAsync(key).ConfigureAwait(false);
			if (exist != null)
			{
				_context.Entry(exist).CurrentValues.SetValues(t);
				await _context.SaveChangesAsync().ConfigureAwait(false);
			}
			return exist;
		}

		public virtual int Count()
		{
			return GetAll().Count();
		}

		public virtual async Task<int> CountAsync()
		{
			return await GetAll().CountAsync().ConfigureAwait(false);
		}

		public virtual void Save()
		{
			_context.SaveChanges();
		}

		public virtual async void SaveChangesAsync()
		{
			await _context.SaveChangesAsync().ConfigureAwait(false);
		}

		public virtual async Task<int> SaveAsync()
		{
			return await _context.SaveChangesAsync().ConfigureAwait(false);
		}

		public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
		{
			return GetAll().Where(predicate);
		}

		public virtual async Task<ICollection<T>> FindByAsync(Expression<Func<T, bool>> predicate)
		{
			return await GetAll().Where(predicate).ToListAsync().ConfigureAwait(false);
		}

		public virtual IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties)
		{
			IQueryable<T> queryable = GetAll();
			foreach (Expression<Func<T, object>> includeProperty in includeProperties)
			{
				queryable = queryable.Include(includeProperty);
			}

			return queryable;
		}

		public virtual T First()
		{
			return GetAll().First();
		}

		public virtual T First(Expression<Func<T, bool>> predicate)
		{
			return GetAll().First(predicate);
		}

		public virtual T FirstOrDefault()
		{
			return GetAll().FirstOrDefault();
		}

		public virtual T FirstOrDefault(Expression<Func<T, bool>> predicate)
		{
			return GetAll().FirstOrDefault(predicate);
		}
		public virtual Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
		{
			return GetAll().FirstOrDefaultAsync(predicate);
		}

		public virtual bool Any()
		{
			return GetAll().Any();
		}

		public virtual bool Any(Expression<Func<T, bool>> predicate)
		{
			return GetAll().Any(predicate);
		}

		private bool _disposed;

		protected virtual void Dispose(bool disposing)
		{
			if (!_disposed)
			{
				if (disposing)
				{
					_context.Dispose();
				}
				_disposed = true;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private IQueryable<T> ApplyUserFilter(IQueryable<T> source)
		{
			if (User == default)
			{
				return source;
			}
			else
			{
				Expression<Func<T, bool>> userProfileFilter = ApplyUserProfileFilter(User);
				if (userProfileFilter == null)
				{
					return source;
				}
				else
				{
					return source.Where(userProfileFilter);
				}
			}
		}

		protected virtual Expression<Func<T, bool>> ApplyUserProfileFilter(Guid user)
		{
			return _ => true;
		}
	}
}
