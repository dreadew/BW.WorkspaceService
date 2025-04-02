using Serilog;
using WorkspaceService.Api.Extensions;
using WorkspaceService.Api.Middlewares;
using WorkspaceService.Application.Extension;
using WorkspaceService.Domain.Constants;
using WorkspaceService.Grpc.Extension;
using WorkspaceService.Infrastructure.Configuration;
using WorkspaceService.Infrastructure.Data;
using WorkspaceService.Infrastructure.Extension;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(options =>
{
    options.Configure(builder.Configuration.GetSection(KestrelConstants.Section));
});
builder.Logging.UseObservabilityLogging(builder.Configuration);
builder.Host.UseSerilog();
builder.Configuration.AddVaultConfiguration(options =>
{
    builder.Configuration.GetSection(VaultConstants.VaultSection).Bind(options);
});
builder.Services.AddSwaggerDocumentation();
builder.Services.AddApiServices();
builder.Services.AddGrpcServices(builder.Configuration);
builder.Services.AddObservability(builder.Configuration, builder.Environment);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

Log.Information("Запуск: Workspace Service");
SwaggerExtensions.LogSwaggerConfiguration(builder.Configuration);
ApiExtensions.LogApiConfiguration(builder.Configuration);
ObservabilityExtensions.LogObservabilityConfiguration(builder.Configuration);

var app = builder.Build();

app.Services.MigrateUp();

app.UseSwaggerWhenDevelopment();
app.UseRequestLogging();
app.MapGrpcService<WorkspaceService.Grpc.Services.WorkspaceService>();
app.MapControllers();
app.UseMiddleware<GrpcAuthMiddleware>();
app.UseCorsAllowAll();

Log.Information("Сервис успешно запущен");
app.Run();