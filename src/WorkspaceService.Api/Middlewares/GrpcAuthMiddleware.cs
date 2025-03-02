using System.Net;
using WorkspaceService.Domain.Services;
using WorkspaceService.Grpc.Services;

namespace WorkspaceService.Api.Middlewares;

public class GrpcAuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IIdentityService _identityService;
    private readonly ILogger<GrpcAuthMiddleware> _logger;

    public GrpcAuthMiddleware(RequestDelegate next, IIdentityService identityService, ILogger<GrpcAuthMiddleware> logger)
    {
        _next = next;
        _identityService = identityService;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string? token = context.Request.Headers["Authorization"];

        if (!string.IsNullOrEmpty(token) && token.StartsWith("Bearer "))
        {
            token = token.Substring("Bearer ".Length).Trim();
        }
        else
        {
            _logger.LogWarning("Отсутствует заголовок Authorization");
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsync("Unauthorized: AccessToken is missing");
            return;
        }
        
        bool isValid = await _identityService.VerifyAsync(token);

        if (!isValid)
        {
            _logger.LogWarning("Неверный AccessToken");
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await context.Response.WriteAsync("Unauthorized: Invalid AccessToken");
            return;
        }
        
        await _next(context);
    }
}