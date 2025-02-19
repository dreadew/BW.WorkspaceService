using DefaultNamespace;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace WorkspaceService.Infrastructure.Data.Interceptors;

public class UpdateAuditableInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken token = default)
    {
        if (eventData.Context is not null)
        {
            UpdateAuditableEntities(eventData.Context);
        }

        return base.SavingChangesAsync(eventData, result, token);
    }

    private static void UpdateAuditableEntities(DbContext context)
    {
        DateTime utcNow = DateTime.UtcNow;
        var entities = context.ChangeTracker.Entries<IAuditable>().ToList();

        foreach (EntityEntry<IAuditable> entry in entities)
        {
            if (entry.State == EntityState.Added)
            {
                SetCurrentPropertyValue(
                    entry, nameof(IAuditable.CreatedAt), utcNow);
            }

            if (entry.State == EntityState.Modified)
            {
                SetCurrentPropertyValue(
                    entry, nameof(IAuditable.ModifiedAt), utcNow);
            }
        }
    }
    
    static void SetCurrentPropertyValue(
        EntityEntry entry,
        string propertyName,
        DateTime utcNow) => entry.Property(propertyName)
        .CurrentValue = utcNow;
}