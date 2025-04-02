using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Quartz;
using WorkspaceService.Domain.Constants;
using WorkspaceService.Domain.Entities;
using WorkspaceService.Domain.Interfaces;
using WorkspaceService.Domain.Options;
using WorkspaceService.Domain.Repositories;
using WorkspaceService.Domain.Services;
using WorkspaceService.Infrastructure.Data;
using WorkspaceService.Infrastructure.Data.Interceptors;
using WorkspaceService.Infrastructure.Jobs;
using WorkspaceService.Infrastructure.Messaging;
using WorkspaceService.Infrastructure.Repositories;
using WorkspaceService.Infrastructure.Services;

namespace WorkspaceService.Infrastructure.Extension;

public static class InfrastructureExtension
{
    public static void AddInfrastructure(this IServiceCollection services, 
        IConfiguration configuration)
    {
        InitSecrets(services, configuration);
        InitDb(services, configuration);
        InitRepositories(services);
        InitServices(services, configuration);
        InitMessaging(services, configuration);
        InitJobs(services);
    }
    
    private static void InitServices(this IServiceCollection services, IConfiguration 
            configuration)
    {
        services.Configure<S3Options>(configuration.GetSection(S3Constants.Section));
        services.AddScoped<IFileService, FileService>();
    }

    private static void InitDb(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DBOptions>(configuration.GetSection(DBConstants.Section));
        services.AddSingleton<UpdateAuditableInterceptor>();
        services.AddDbContextFactory<ApplicationDbContext>((provider, options) =>
        {
            var dbOptions = provider.GetRequiredService<IOptions<DBOptions>>().Value;
            
            options.UseNpgsql(dbOptions.PostgresConnection)
                .AddInterceptors(new UpdateAuditableInterceptor())
                .UseLazyLoadingProxies();
        });
    }

    private static void InitMessaging(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<KafkaOptions>(
            configuration.GetSection(KafkaConstants.Section));
        services.AddSingleton<IKafkaProducerService, KafkaProducerService>();
        services.AddHostedService<KafkaConsumerService>();
    }

    private static void InitJobs(this IServiceCollection services)
    {
        services.AddQuartz(q =>
        {
            var jobKey = new JobKey(nameof(SendMessageJob));
                
            q.AddJob<SendMessageJob>(opts => opts.WithIdentity(jobKey));
            q.AddTrigger(opts => opts
                .ForJob(jobKey)
                .WithIdentity($"{nameof(SendMessageJob)}-trigger")
                .WithCronSchedule("0/15 * * * * ?"));
        });
        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
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

    private static void InitSecrets(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<VaultOptions>(
            configuration.GetSection(VaultConstants.VaultSection));
        services.AddSingleton<IVaultService, VaultService>();
        //services.AddSingleton<ISecretsProvider, InfiscalSecretsProvider>();
    }
}