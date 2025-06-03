using Serilog;

namespace WorkspaceService.Api.Extensions;

public static class ApplicationExtensions
{
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
    
    public static WebApplication UseCorsAllowAll(this WebApplication app)
    {
        app.UseCors(b => 
            b.WithOrigins("http://localhost:8080", "http://100.78.246.62:8080", "http://0.0.0.0:8080")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
        );
        
        return app;
    }
}