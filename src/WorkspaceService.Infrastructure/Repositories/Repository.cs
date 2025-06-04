using System.Linq.Expressions;
using DefaultNamespace;
using Microsoft.EntityFrameworkCore;
using WorkspaceService.Domain.DTOs;
using WorkspaceService.Domain.Repositories;
using WorkspaceService.Infrastructure.Data;

namespace WorkspaceService.Infrastructure.Repositories;

public class Repository<TEntity> : IRepository<TEntity> 
    where TEntity : class, IEntity<Guid>
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

    public bool Update(TEntity entity, 
        CancellationToken cancellationToken = default)
    {
        _dbContext.Update(entity);
        return true;
    }
    
    public IQueryable<TEntity> FindMany(Expression<Func<TEntity, bool>> predicate)
    {
        return _dbContext.Set<TEntity>()
            .Where(predicate)
            .OrderBy(x => x.Id);
    }
    
    public IQueryable<TEntity> GetAll()
    {
        return _dbContext.Set<TEntity>();
    }
}