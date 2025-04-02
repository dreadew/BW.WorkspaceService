namespace WorkspaceService.Domain.Options;

public class RateLimiterOptions
{
    public int Window { get; set; }
    public int PermitLimit { get; set; }
    public int QueueLimit { get; set; }
}