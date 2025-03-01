using System.Linq.Expressions;
using WorkspaceService.Domain.DTOs;

namespace WorkspaceService.Domain.Repositories;

public interface IRepository<T> 
    where T : class
{
    Task CreateAsync(
        T entity,
        CancellationToken cancellationToken = default);

    Task CreateManyAsync(IEnumerable<T> entities,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task<bool> UpdateAsync(
        T entity,
        CancellationToken cancellationToken = default);
    
    Task<IReadOnlyCollection<T>> ListAsync(
        ListRequest listParams,
        CancellationToken cancellationToken = default);
    
    Task<T?> GetByIdAsync(
        string id,
        CancellationToken cancellationToken = default);

    Task<T?> FindAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<T>> FindManyAsync(
        Expression<Func<T, bool>> predicate,
        CancellationToken cancellationToken = default);
}