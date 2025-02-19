namespace WorkspaceService.Domain.Constants;

public static class RateLimiterConstants
{
    private const string RateLimiterSection = "RateLimiter";
    public const string RateLimiterWindow = $"{RateLimiterSection}-Window";
    public const string RateLimiterPermitLimit = $"{RateLimiterSection}-PermitLimit";
    public const string RateLimiterQueueLimit = $"{RateLimiterSection}-QueueLimit";
}