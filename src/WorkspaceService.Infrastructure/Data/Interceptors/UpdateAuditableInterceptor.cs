using DefaultNamespace;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using WorkspaceService.Domain.Context;

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
                SetCurrentDatePropertyValue(
                    entry, nameof(IAuditable.CreatedAt), utcNow);
                SetCurrentPropertyValue(
                    entry, nameof(IAuditable.ChangedBy), CurrentUserContext.CurrentUserId);
            }

            if (entry.State == EntityState.Modified)
            {
                SetCurrentDatePropertyValue(
                    entry, nameof(IAuditable.ModifiedAt), utcNow);
                SetCurrentPropertyValue(
                    entry, nameof(IAuditable.ChangedBy), CurrentUserContext.CurrentUserId);
            }
        }
    }
    
    private static void SetCurrentDatePropertyValue(
        EntityEntry entry,
        string propertyName,
        DateTime utcNow) => entry.Property(propertyName)
        .CurrentValue = utcNow;
    
    private static void SetCurrentPropertyValue(
        EntityEntry entry,
        string propertyName,
        string value) => entry.Property(propertyName).CurrentValue = value;
}