namespace CutiApp.Repositories
{
    public interface IRepository<T> where T: class
    {
        Task<T?> GetByIdAsync(long id, params string[] includeProperties);
        Task<List<T>> GetAllAsync(params string[] includeProperties);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
