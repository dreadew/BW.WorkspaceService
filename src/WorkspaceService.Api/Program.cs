using FluentValidation.AspNetCore;
using Grpc.Net.Client;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using Serilog;
using WorkspaceService.Api.Config;
using WorkspaceService.Api.Middlewares;
using WorkspaceService.Application.Extension;
using WorkspaceService.Domain.Excpetions;
using WorkspaceService.Domain.Services;
using WorkspaceService.Grpc.Services;
using WorkspaceService.Infrastructure.Data;
using WorkspaceService.Infrastructure.Extension;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));

builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddProblemDetails();
builder.Services.AddCors();

try
{
    builder.Services.AddInfrastructure();
    builder.Services.AddApplication();
}
catch (VariableNotFoundException e)
{
    Console.WriteLine($"Не удалось найти переменную ${e.VariableName} при создании ${e.ServiceName}");
    throw;
}

builder.Services.AddSingleton<IConfigureOptions<RateLimiterOptions>, 
    ConfigureRateLimiterOptions>();
builder.Services.AddRateLimiter();

var grpcChannel = GrpcChannel.ForAddress(builder.Configuration["IdentityServiceUrl"]);

builder.Services.AddSingleton(grpcChannel);
builder.Services.AddSingleton<IIdentityService, GrpcIdentityServiceClient>();

var app = builder.Build();

app.Services.MigrateUp();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseMiddleware<GrpcAuthMiddleware>();

app.MapControllers();

app.UseCors(builder => 
    builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyMethod()
);

app.Run();