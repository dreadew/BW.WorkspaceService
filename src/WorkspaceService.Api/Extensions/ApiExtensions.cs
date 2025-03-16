using System.Diagnostics;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using Serilog;
using WorkspaceService.Api.Config;
using WorkspaceService.Api.Filters;

namespace WorkspaceService.Api.Extensions;

public static class ApiExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = ctx =>
            {
                ctx.ProblemDetails
                    .Extensions.Add("traceId", Activity.Current?.Id ?? ctx.HttpContext.TraceIdentifier);
            };
        });
        services.AddControllers(options =>
        {
            options.Filters.Add<ApiExceptionFilterAttribute>();
            options.Filters.Add<RouteDataToModelFilter>();
        });
        services.AddFluentValidationAutoValidation();
        services.AddSingleton<IConfigureOptions<RateLimiterOptions>, ConfigureRateLimiterOptions>();
        services.AddRateLimiter();
        services.AddCors();
        services.AddGrpc();
        
        return services;
    }

    public static void LogApiConfiguration(IConfiguration configuration)
    {
        Log.Information("Конфигурация API:");
        Log.Information("  - Problem Details с TraceId");
        Log.Information("  - Обработчик ошибок через ApiExceptionFilterAttribute");
        Log.Information("  - Добавление Id из маршрута через RouteDataToModelFilter");
        Log.Information("  - Автоматическая валидация через FluentValidation");
        
        var rateLimit = configuration.GetSection("RateLimiting:PermitLimit").Value ?? "20";
        var window = configuration.GetSection("RateLimiting:Window").Value ?? "10s";
        Log.Information("  - Rate limiting: {Limit} запросов в {Window}", rateLimit, window);
    }
}