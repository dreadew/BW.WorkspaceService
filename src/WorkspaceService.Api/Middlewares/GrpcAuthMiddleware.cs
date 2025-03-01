using System.Net;
using WorkspaceService.Grpc.Services;

namespace WorkspaceService.Api.Middlewares;

public class GrpcAuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly GrpcIdentityServiceClient _grpcIdentityServiceClient;
    private readonly ILogger<GrpcAuthMiddleware> _logger;

    public GrpcAuthMiddleware(RequestDelegate next, GrpcIdentityServiceClient grpcIdentityServiceClient, ILogger<GrpcAuthMiddleware> logger)
    {
        _next = next;
        _grpcIdentityServiceClient = grpcIdentityServiceClient;
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
        
        bool isValid = await _grpcIdentityServiceClient.VerifyTokenAsync(token);

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