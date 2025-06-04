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

    bool Update(
        T entity,
        CancellationToken cancellationToken = default);

    IQueryable<T> FindMany(
        Expression<Func<T, bool>> predicate);

    IQueryable<T> GetAll();
}