using System.Diagnostics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;

namespace WorkspaceService.Api.Extensions;

public static class ObservabilityExtensions
{
    public static IServiceCollection AddObservability(this IServiceCollection services, IConfiguration configuration, string serviceName = "UsersService")
    {
        services.AddOpenTelemetry()
            .WithTracing(providerBuilder =>
            {
                providerBuilder
                    .AddAspNetCoreInstrumentation()
                    .AddSource(serviceName)
                    .SetResourceBuilder(ResourceBuilder.CreateDefault()
                        .AddService(serviceName, "1.0"))
                    .AddJaegerExporter(opts =>
                    {
                        opts.AgentHost = configuration["Jaeger:AgentHost"] ?? "localhost";
                        opts.AgentPort = int.Parse(configuration["Jaeger:AgentPort"] ?? "6831");
                    });
            });
            
        services.AddSingleton(new ActivitySource(serviceName));
        
        return services;
    }
    
    public static ILoggingBuilder UseObservabilityLogging(this ILoggingBuilder builder, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            .CreateLogger();
            
        return builder.AddSerilog();
    }
    
    public static void LogObservabilityConfiguration(IConfiguration configuration)
    {
        var serilogSinks = new List<string>();
        
        if (configuration["Serilog:WriteTo"] != null)
        {
            var section = configuration.GetSection("Serilog:WriteTo");
            foreach (var child in section.GetChildren())
            {
                var name = child["Name"];
                if (!string.IsNullOrEmpty(name))
                {
                    serilogSinks.Add(name);
                }
            }
        }
        
        Log.Information("Конфигурация Serilog: {Sinks}", string.Join(", ", serilogSinks));
        
        var jaegerHost = configuration["Jaeger:AgentHost"] ?? "localhost";
        var jaegerPort = configuration["Jaeger:AgentPort"] ?? "6831";
        
        Log.Information("Конфигурация Jaeger:");
        Log.Information("  - Host: {Host}", jaegerHost);
        Log.Information("  - Port: {Port}", jaegerPort);
        Log.Information("  - UI: http://{Host}:16686", jaegerHost);
    }
}