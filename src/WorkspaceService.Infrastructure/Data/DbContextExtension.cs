using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WorkspaceService.Infrastructure.Data;

public static class DbContextExtension
{
    public static void MigrateUp(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();
        var logger = scope.ServiceProvider
            .GetRequiredService<ILogger<ApplicationDbContext>>();
        try
        {
            logger.LogDebug("Применяем миграции...");
            context.Database.Migrate();
            logger.LogDebug("Миграции были успешно применены.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message, "Произошла ошибка при применении миграций");
            throw;
        }
    }
}