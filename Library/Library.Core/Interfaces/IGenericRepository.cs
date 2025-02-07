namespace Library.Core.Interfaces;

public interface IGenericRepository<T> : IGenericReadRepository<T> where T : class
{
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    Task <IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}