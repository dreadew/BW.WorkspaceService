using System.Net;
using WorkspaceService.Domain.Constants;
using WorkspaceService.Domain.Context;
using WorkspaceService.Domain.Services;
using WorkspaceService.Grpc.Services;

namespace WorkspaceService.Api.Middlewares;

public class GrpcAuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IIdentityServiceClient _identityServiceClient;
    private readonly ILogger<GrpcAuthMiddleware> _logger;
    public GrpcAuthMiddleware(RequestDelegate next, 
        IIdentityServiceClient identityServiceClient,
        ILogger<GrpcAuthMiddleware> logger)
    {    
        _next = next;
        _identityServiceClient = identityServiceClient;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string? token = context.Request.Headers["Authorization"];

        if (!string.IsNullOrEmpty(token) && token.StartsWith("Bearer "))
        {
            token = token.Substring("Bearer ".Length).Trim();
        }

        var (isValid, userId) = await _identityServiceClient.VerifyAsync(token);

        if (!isValid || string.IsNullOrEmpty(userId))
        {
            _logger.LogWarning("Неверный AccessToken");
            await context.Response.WriteAsync(ExceptionResourceKeys.InvalidAccessToken);
        }

        CurrentUserContext.CurrentUserId = userId;
        context.Items["FromId"] = userId;
        
        await _next(context);
    }
}