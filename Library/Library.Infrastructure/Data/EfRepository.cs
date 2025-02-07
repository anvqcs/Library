using Library.Core.Common.Exceptions;
using Library.Core.Entities;
using Library.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Library.Infrastructure.Data;

public class EfRepository<T> : IGenericRepository<T> where T : class
{
    private readonly LibraryDbContext _dbContext;

    public EfRepository(LibraryDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<T>().Add(entity);

        await SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities,
        CancellationToken cancellationToken = default)
    {
        // ReSharper disable once PossibleMultipleEnumeration
        _dbContext.Set<T>().AddRange(entities);

        await SaveChangesAsync(cancellationToken);

        // ReSharper disable once PossibleMultipleEnumeration
        return entities;
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<T>().Update(entity);

        await SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        if (entity is BaseEntity baseEntity)
        {
            baseEntity.IsDeleted = true;

            _dbContext.Entry(entity).State = EntityState.Modified;
        }
        else
        {
            throw new NotFoundException($"{nameof(T)} not found.");
        }

        await SaveChangesAsync(cancellationToken);
    }


    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<T?> GetByIdAsync<TId>(TId id, CancellationToken cancellationToken = default) where TId : notnull
    {
        return await _dbContext.Set<T>().FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<IEnumerable<T>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<T>().ToListAsync(cancellationToken);
    }
}