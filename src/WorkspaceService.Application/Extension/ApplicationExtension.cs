

using Microsoft.Extensions.DependencyInjection;

namespace WorkspaceService.Application.Extension;

public static class ApplicationExtension
{
    /// <summary>
    /// Внедрение зависимостей слоя Application
    /// </summary>
    /// <param name="services"></param>
    public static void AddApplication(this IServiceCollection services)
    {
        InitMappers(services);
        InitValidators(services);
        InitServices(services);
    }

    /// <summary>
    /// Внедрение зависимостей маппинга
    /// </summary>
    /// <param name="services"></param>
    private static void InitMappers(this IServiceCollection services)
    {
    }

    /// <summary>
    /// Внедрение зависимостей валидаций
    /// </summary>
    /// <param name="services"></param>
    private static void InitValidators(this IServiceCollection services)
    {
    }

    /// <summary>
    /// Внедрение зависимостей сервисов
    /// </summary>
    /// <param name="services"></param>
    private static void InitServices(this IServiceCollection services)
    {
    }
}