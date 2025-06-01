using System.Diagnostics;
using OpenTelemetry;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;

namespace WorkspaceService.Api.Extensions;

public static class ObservabilityExtensions
{
    public static IServiceCollection AddObservability(this IServiceCollection services,
        IConfiguration configuration, IWebHostEnvironment environment)
    {
        var enabled = configuration.GetValue<bool>("Jaeger:Enabled");
        if (!enabled)
        {
            return services;
        }
        
        var agentHost = configuration.GetValue<string>("Jaeger:AgentHost");
        var agentPort = configuration.GetValue<int>("Jaeger:AgentPort");
        var protocol = configuration.GetValue<JaegerExportProtocol>("Jaeger:Protocol");
        var endpoint = configuration.GetValue<string>("Jaeger:Endpoint");
        var processorType = configuration.GetValue<ExportProcessorType>("Jaeger:ProcessorType");
        var maxPayloadSize = configuration.GetValue<int>("Jaeger:MaxPayloadSize");
        var serviceName = configuration.GetValue<string>("Jaeger:ServiceName");
        
        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(serviceName!).AddTelemetrySdk())
            .WithTracing(providerBuilder =>
            {
                providerBuilder
                    .SetSampler(new AlwaysOnSampler())
                    .AddAspNetCoreInstrumentation()
                    .AddJaegerExporter(opts =>
                    {
                        opts.AgentHost = agentHost;
                        opts.AgentPort = agentPort;
                        opts.Endpoint = new Uri(endpoint);
                        opts.MaxPayloadSizeInBytes = maxPayloadSize;
                        opts.Protocol = protocol; 
                        opts.ExportProcessorType = processorType;
                    });
            });
            
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