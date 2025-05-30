using System.Linq.Expressions;
using DefaultNamespace;
using Microsoft.EntityFrameworkCore;
using WorkspaceService.Domain.DTOs;
using WorkspaceService.Domain.Repositories;
using WorkspaceService.Infrastructure.Data;

namespace WorkspaceService.Infrastructure.Repositories;

public class Repository<TEntity> : IRepository<TEntity> 
    where TEntity : class, IEntity<string>
{
    private readonly ApplicationDbContext _dbContext;

    public Repository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task CreateAsync(TEntity entity, 
        CancellationToken cancellationToken = default)
    {
        await _dbContext.Set<TEntity>()
            .AddAsync(entity, cancellationToken);
    }
    
    public async Task CreateManyAsync(IEnumerable<TEntity> entities, 
        CancellationToken cancellationToken = default)
    {
        await _dbContext.Set<TEntity>()
            .AddRangeAsync(entities, cancellationToken);
    }

    public async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate, 
        CancellationToken cancellationToken = default)
    {
        await _dbContext.Set<TEntity>()
            .Where(predicate)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<bool> UpdateAsync(TEntity entity, 
        CancellationToken cancellationToken = default)
    {
        var exists = await _dbContext.Set<TEntity>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == entity.Id, 
                    cancellationToken)
            is not null;

        if (!exists)
        {
            return false;
        }

        _dbContext.Entry(entity).State = EntityState.Modified;
        return true;
    }

    public IQueryable<TEntity> Paging(ListRequest listParams)
    {
        return _dbContext.Set<TEntity>()
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .Skip(listParams.Offset)
            .Take(listParams.Limit);
    }
    
    public IQueryable<TEntity> FindMany(Expression<Func<TEntity, bool>> predicate)
    {
        return _dbContext.Set<TEntity>()
            .AsNoTracking()
            .Where(predicate)
            .OrderBy(x => x.Id);
    }
}