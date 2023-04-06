using EFCore.BulkExtensions;
using Microsoft.EntityFrameworkCore;
using Real_Estate_IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Real_Estate_Services
{

    public abstract class Repository<C, T> : IRepository<T> where T : class where C : Microsoft.EntityFrameworkCore.DbContext, new()
    {
        private C _entities = new C();
        public C Context
        {
            get { return _entities; }
            set { _entities = value; }

        }
         
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _entities.Set<T>().ToListAsync();
        }
        public async Task<T> GetByIdAsync(int id)
        {
            return await _entities.Set<T>().FindAsync(id);
        }
        public async Task AddAsync(T entity)
        {
            await _entities.Set<T>().AddAsync(entity);
            await _entities.SaveChangesAsync();
        }
        public async Task UpdateAsync(T entity)
        {
            _entities.Set<T>().Update(entity);
            await _entities.SaveChangesAsync();
        }
        public async Task DeleteAsync(T entity)
        {
            _entities.Set<T>().Remove(entity);
            await _entities.SaveChangesAsync();
        }
        public async Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize)
        {
            return await _entities.Set<T>()
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync();
        }
        public async Task<IEnumerable<T>> SearchAsync(Expression<Func<T, bool>> predicate)
        {
            return await _entities.Set<T>().Where(predicate).ToListAsync();
        }
        public async Task<int> CountAsync()
        {
            return await _entities.Set<T>().CountAsync();
        }
        public async Task<bool> ExistAsync(int id)
        {
            return await _entities.Set<T>().FindAsync(id) != null;
        }
        public async Task<IEnumerable<T>> GetAllIncludingAsync(params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _entities.Set<T>();
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
            return await query.ToListAsync();
        }
        public async Task SaveAsync()
        {
            await _entities.SaveChangesAsync();
        }
        public async Task ExecuteInTransactionAsync(Func<Task> function)
        {
            using (var transaction = _entities.Database.BeginTransaction())
            {
                try
                {
                    await function();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await _entities.Set<T>().FirstOrDefaultAsync(predicate);
        }
        public async Task DeleteByIdAsync(int id)
        {
            var entity = await _entities.Set<T>().FindAsync(id);
            if (entity != null)
            {
                _entities.Set<T>().Remove(entity);
                await _entities.SaveChangesAsync();
            }
        }
        public async Task<int> ExecuteSqlCommandAsync(string sql, params object[] parameters)
        {
            return await _entities.Database.ExecuteSqlRawAsync(sql, parameters);
        }
        public async Task<IEnumerable<T>> GetFromSqlAsync(string sql, params object[] parameters)
        {
            return await _entities.Set<T>().FromSqlRaw(sql, parameters).ToListAsync();
        }
        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _entities.Set<T>().AddRangeAsync(entities);
            await _entities.SaveChangesAsync();
        }
        
        public IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
        {
            var query = _entities.Set<T>().Where(predicate);
            return query;
        }
       
    }
}
