using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WorkspaceService.Domain.Constants;
using WorkspaceService.Domain.Entities;
using WorkspaceService.Domain.Interfaces;
using WorkspaceService.Domain.Repositories;
using WorkspaceService.Infrastructure.Data;
using WorkspaceService.Infrastructure.Data.Interceptors;
using WorkspaceService.Infrastructure.Repositories;
using WorkspaceService.Infrastructure.Services;

namespace WorkspaceService.Infrastructure.Extension;

public static class InfrastructureExtension
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        InitSecrets(services);
        InitDb(services);
        InitRepositories(services);
        InitServices(services);
    }
    
    private static void InitServices(this IServiceCollection services)
    {
        services.AddScoped<IFileService, FileService>();
    }

    private static void InitDb(this IServiceCollection services)
    {
        services.AddSingleton<UpdateAuditableInterceptor>();
        services.AddDbContextFactory<ApplicationDbContext>((provider, options) =>
        {
            var secretsProvider = provider.GetService<ISecretsProvider>();
            var connectionString = secretsProvider?.GetSecret(
                DBConstants.DefaultConnectionString,
                "dev");
            options.UseNpgsql(connectionString)
                .AddInterceptors(new UpdateAuditableInterceptor())
                .UseLazyLoadingProxies();
        });
    }

    private static void InitRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IRepository<Workspaces>, Repository<Workspaces>>();
        services.AddScoped<IRepository<WorkspaceRoles>, Repository<WorkspaceRoles>>();
        services.AddScoped<IRepository<WorkspaceRoleClaims>, Repository<WorkspaceRoleClaims>>();
        services.AddScoped<IRepository<WorkspacePositions>, Repository<WorkspacePositions>>();
        services.AddScoped<IRepository<WorkspaceDirectory>, Repository<WorkspaceDirectory>>();
        services.AddScoped<IRepository<WorkspaceDirectoryArtifact>, Repository<WorkspaceDirectoryArtifact>>();
    }

    private static void InitSecrets(this IServiceCollection services)
    {
        services.AddSingleton<ISecretsProvider, InfiscalSecretsProvider>();
    }
}