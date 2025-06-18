namespace financial_control_domain.Interfaces.Repositories;

public interface IRepositoryBase<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync(
        int page = 1,
        int itemsPeerPage = 10,
        CancellationToken cancellationToken = default
    );
    Task<T?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
    Task<int> InsertAsync(T entity, CancellationToken cancellationToken = default);
    Task<int> UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task<int> DeleteAsync(T entity, CancellationToken cancellationToken = default);
}
