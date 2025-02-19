using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using WorkspaceService.Domain.Constants;
using WorkspaceService.Domain.Interfaces;

namespace WorkspaceService.Api.Config;

public class ConfigureRateLimiterOptions : IConfigureOptions<RateLimiterOptions>
{
    private readonly ISecretsProvider _secretsProvider;

    public ConfigureRateLimiterOptions(ISecretsProvider secretsProvider)
    {
        _secretsProvider = secretsProvider;
    }

    public void Configure(string? name, RateLimiterOptions options)
    {
        var windowString = _secretsProvider.GetSecret(
            RateLimiterConstants.RateLimiterWindow, 
            "dev") ?? "5";
        var permitLimitString = _secretsProvider.GetSecret(
            RateLimiterConstants.RateLimiterPermitLimit,
            "Development") ?? "2";
        var queueLimitString = _secretsProvider.GetSecret(
            RateLimiterConstants.RateLimiterQueueLimit, 
            "Development") ?? "2";
        
        options.AddFixedWindowLimiter("Fixed", limiterOptions =>
        {
            limiterOptions.Window = TimeSpan.FromSeconds(int.Parse(windowString));
            limiterOptions.PermitLimit = int.Parse(permitLimitString);
            limiterOptions.QueueLimit = int.Parse(queueLimitString);
        });
    }

    public void Configure(RateLimiterOptions options) => Configure(Options.DefaultName, options);
}