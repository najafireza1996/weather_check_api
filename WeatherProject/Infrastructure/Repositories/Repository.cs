using System;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WeatherProject.Infrastructure.Data;
using WeatherProject.Infrastructure.IRepositories;

namespace WeatherProject.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly AppDbContext _appContext;
        internal DbSet<T> dbSet;

        public Repository(AppDbContext appContext)
        {
            _appContext = appContext;
            dbSet = _appContext.Set<T>();
        }

        public async Task<T> GetOneAsync(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = true)
        {
            IQueryable<T> query = tracked ? dbSet.AsTracking() : dbSet.AsNoTracking();
            query = query.Where(filter);
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }
            return await query.ToListAsync();
        }

        public async Task AddAsync(T entity)
        {
            await dbSet.AddAsync(entity);
        }

        public Task RemoveAsync(T entity)
        {
            dbSet.Remove(entity);
            return Task.CompletedTask;
        }

        public Task RemoveRangeAsync(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(T entity)
        {
            dbSet.Update(entity);
            return Task.CompletedTask;
        }
    }
}

