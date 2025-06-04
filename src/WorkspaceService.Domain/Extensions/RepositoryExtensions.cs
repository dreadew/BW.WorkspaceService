using System.Linq.Expressions;
using WorkspaceService.Domain.DTOs;

namespace WorkspaceService.Domain.Extensions;

public static class RepositoryExtensions
{
    public static IQueryable<TEntity> WhereIf<TEntity>(this IQueryable<TEntity> entities, bool condition, Expression<Func<TEntity, bool>> predicate)
    {
        if (condition)
            return entities.Where(predicate);
        return entities;
    }

    public static IQueryable<TEntity> Paging<TEntity>(this IQueryable<TEntity> entities, ListRequest dto)
    {
        return entities.Skip(dto.Offset).Take(dto.Limit);
    }
}