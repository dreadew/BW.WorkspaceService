using Serilog;

namespace WorkspaceService.Api.Extensions;

public static class ApplicationExtensions
{
    private static readonly string[] SupportedCultures = { "en", "ru" };
    public static WebApplication UseSwaggerWhenDevelopment(this WebApplication app)
    {
        if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => 
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Workspace API v1");
                c.RoutePrefix = string.Empty;
            });
        }
        
        return app;
    }
    
    public static WebApplication UseRequestLogging(this WebApplication app)
    {
        app.UseSerilogRequestLogging(options =>
        {
            options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
            {
                diagnosticContext.Set("RequestId", httpContext.TraceIdentifier);
                diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"].ToString());
                diagnosticContext.Set("RemoteIp", httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown");
            };
        });
        
        return app;
    }
    
    public static WebApplication UseCorsFromConfig(this WebApplication app)
    {
        var config = app.Configuration;
        var corsUrls = config["CorsUrls"]?
                           .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                       ?? Array.Empty<string>();

        app.UseCors(b =>
            b.WithOrigins(corsUrls)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
        );
        return app;
    }
    
    public static WebApplication UseLocalizationFromConfig(this WebApplication app)
    {
        var config = app.Configuration;
        var locale = config["Culture"]?.ToLower() ?? "ru";
        if (!Array.Exists(SupportedCultures, c => c.Equals(locale, StringComparison.OrdinalIgnoreCase)))
        {
            locale = "ru";
        }
        var localizationOptions = new RequestLocalizationOptions()
            .SetDefaultCulture(locale)
            .AddSupportedCultures(SupportedCultures)
            .AddSupportedUICultures(SupportedCultures);
        app.UseRequestLocalization(localizationOptions);
        
        return app;
    }
}