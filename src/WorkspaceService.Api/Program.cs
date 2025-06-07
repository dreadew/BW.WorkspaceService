using Common.AspNetCore.Middlewares;
using Common.Base.Constants;
using Common.Base.Converters;
using Common.Services.Configuration;
using Newtonsoft.Json;
using Serilog;
using WorkspaceService.Api.Extensions;
using WorkspaceService.Application.Extension;
using WorkspaceService.Grpc.Extension;
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
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddHealthChecks();
builder.Services.AddResponseCompression();
builder.Services.AddSwaggerDocumentation();
builder.Services.AddApiServices();
builder.Services.AddGrpcServices(builder.Configuration);
builder.Services.AddObservability(builder.Configuration, builder.Environment);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        options.SerializerSettings.Converters.Add(new CustomDateTimeConverter());
        options.SerializerSettings.Converters.Add(new CustomDateOnlyConverter());
    });

if (!Directory.Exists("logs"))
{
    Directory.CreateDirectory("logs");
}

Log.Information("Запуск сервиса рабочих пространств");
SwaggerExtensions.LogSwaggerConfiguration(builder.Configuration);
ApiExtensions.LogApiConfiguration(builder.Configuration);
ObservabilityExtensions.LogObservabilityConfiguration(builder.Configuration);

var app = builder.Build();

app.Services.MigrateUp();

app.UseLocalizationFromConfig();
app.UseCorsFromConfig();
app.UseResponseCompression();

app.UseSwaggerWhenDevelopment();
app.UseRequestLogging();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseMiddleware<GrpcAuthMiddleware>();

app.MapControllers();
app.MapGrpcService<WorkspaceService.Grpc.Services.WorkspaceService>();

app.UseHealthChecks("/health");

Log.Information("Сервис успешно запущен");

app.Run();