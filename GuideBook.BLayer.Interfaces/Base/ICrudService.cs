namespace GuideBook.BLayer;

public interface ICrudService<T>
{
    Task<T> CreateAsync(T entity);
    Task DeleteAsync(T entity);
}
