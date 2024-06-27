using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Application.Common.Interfaces;
using WhiteLagoon.Infrastructure.Data;

namespace WhiteLagoon.Infrastructure.Repository
{
	public class Repository<T> : IRepository<T> where T : class
	{
		private readonly ApplicationDbContext _db;

		internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db=db;
			dbSet = _db.Set<T>();
        }

		public bool Any(Expression<Func<T, bool>> filter)
		{
			return dbSet.Any(filter);
		}

		void IRepository<T>.Add(T entity)
		{
			dbSet.Add(entity);
		}

		T IRepository<T>.Get(Expression<Func<T, bool>> filter, string? includeproperties)
		{
			IQueryable<T> query = dbSet;
			if(filter !=null)
			{
				query = query.Where(filter);
			}
			if(!string.IsNullOrEmpty(includeproperties))
			{
				foreach(var includeprop in includeproperties.Split(new char[] {','},StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(includeprop);
				}
			}
			return query.FirstOrDefault();
		}

		IEnumerable<T> IRepository<T>.GetAll(Expression<Func<T, bool>>? filter, string? includeproperties)
		{
			IQueryable<T> query = dbSet;
			if (filter != null)
			{
				query = query.Where(filter);
			}
			if (!string.IsNullOrEmpty(includeproperties))
			{
				foreach (var includeprop in includeproperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(includeprop);
				}
			}
			return query.ToList();
		}

		void IRepository<T>.Remove(T entity)
		{
			dbSet.Remove(entity);
		}
	}
}
