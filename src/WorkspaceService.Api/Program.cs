using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using Serilog;
using WorkspaceService.Api.Config;
using WorkspaceService.Api.Middlewares;
using WorkspaceService.Application.Extension;
using WorkspaceService.Domain.Excpetions;
using WorkspaceService.Grpc.Extension;
using WorkspaceService.Infrastructure.Data;
using WorkspaceService.Infrastructure.Extension;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddCors();

try
{
    builder.Services.AddInfrastructure();
    builder.Services.AddApplication();
    builder.Services.AddGrpcServices(builder.Configuration);
}
catch (VariableNotFoundException e)
{
    Console.WriteLine($"Не удалось найти переменную ${e.VariableName} при создании ${e.ServiceName}");
}

builder.Services.AddSingleton<IConfigureOptions<RateLimiterOptions>, 
    ConfigureRateLimiterOptions>();
builder.Services.AddRateLimiter();

var app = builder.Build();

app.Services.MigrateUp();

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<GrpcAuthMiddleware>();

app.MapControllers();

app.UseCors(b => 
    b.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyMethod()
);

app.Run();