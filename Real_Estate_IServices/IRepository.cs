using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Real_Estate_IServices
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize);
        Task<IEnumerable<T>> SearchAsync(Expression<Func<T, bool>> predicate);
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        Task<int> CountAsync();
        Task<bool> ExistAsync(int id);
        Task<IEnumerable<T>> GetAllIncludingAsync(params Expression<Func<T, object>>[] includeProperties);
        Task SaveAsync();
        Task ExecuteInTransactionAsync(Func<Task> function);
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate);
        Task DeleteByIdAsync(int id);
        Task<int> ExecuteSqlCommandAsync(string sql, params object[] parameters);
        Task<IEnumerable<T>> GetFromSqlAsync(string sql, params object[] parameters);
        Task AddRangeAsync(IEnumerable<T> entities);

    }
}
