using Common.Base.Constants;
using Common.Base.Options;
using Common.Base.Services;
using Common.Services.Interceptors;
using Common.Services.Messaging;
using Common.Services.ServiceExtensions;
using Common.Services.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Quartz;
using WorkspaceService.Domain.Entities;
using WorkspaceService.Infrastructure.Data;
using WorkspaceService.Infrastructure.Jobs;

namespace WorkspaceService.Infrastructure.Extension;

public static class InfrastructureExtension
{
    public static void AddInfrastructure(this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.AddVault(configuration);
        InitDb(services, configuration);
        InitRepositories(services);
        services.AddUnitOfWork<ApplicationDbContext>();
        services.AddFileService(configuration);
        InitServices(services, configuration);
        services.AddKafkaProducer(configuration);
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
        //services.AddHostedService<KafkaConsumerService>();
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
        services.AddScoped<IRepository<Workspace>, Repository<ApplicationDbContext, Workspace>>();
        services.AddScoped<IRepository<WorkspaceRole>, Repository<ApplicationDbContext, WorkspaceRole>>();
        services.AddScoped<IRepository<WorkspaceRoleClaim>, Repository<ApplicationDbContext, WorkspaceRoleClaim>>();
        services.AddScoped<IRepository<WorkspacePosition>, Repository<ApplicationDbContext, WorkspacePosition>>();
        services.AddScoped<IRepository<WorkspaceDirectory>, Repository<ApplicationDbContext, WorkspaceDirectory>>();
        services.AddScoped<IRepository<WorkspaceDirectoryArtifact>, Repository<ApplicationDbContext, WorkspaceDirectoryArtifact>>();
    }
}