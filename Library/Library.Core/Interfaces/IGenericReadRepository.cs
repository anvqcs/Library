namespace Library.Core.Interfaces;

public interface IGenericReadRepository<T> where T : class
{
    Task<IEnumerable<T>> ListAsync(CancellationToken cancellationToken = default);
    Task<T?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default) where TId : notnull;
}