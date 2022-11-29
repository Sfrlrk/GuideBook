using System.Linq.Expressions;

namespace GuideBook.Repository.Interfaces;
public interface IRepository<T> where T : class
{
    IQueryable<T> Get(Expression<Func<T, bool>> predicate = null);
    Task<T> GetAsync(Expression<Func<T, bool>> predicate);
    Task<T> GetByIdAsync(Guid id);
    Task<T> AddAsync(T entity);
    Task<bool> AddRangeAsync(IEnumerable<T> entities);
    Task<T> UpdateAsync(Guid id, T entity);
    Task<T> UpdateAsync(T entity, Expression<Func<T, bool>> predicate);
    Task<T> DeleteAsync(T entity);
    Task<T> DeleteAsync(Guid id);
    Task<T> DeleteAsync(Expression<Func<T, bool>> predicate);
    Task<List<T>> GetAll(Expression<Func<T, bool>> filter = null);
}
