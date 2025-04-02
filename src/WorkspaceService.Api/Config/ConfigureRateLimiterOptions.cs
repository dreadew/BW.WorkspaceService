using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using WorkspaceService.Domain.Constants;
using WorkspaceService.Domain.Interfaces;

namespace WorkspaceService.Api.Config;

public class ConfigureRateLimiterOptions : IConfigureOptions<RateLimiterOptions>
{
    private readonly IOptions<Domain.Options.RateLimiterOptions>  _options;
    public ConfigureRateLimiterOptions(IOptions<Domain.Options.RateLimiterOptions> options)
    {
        _options = options;
    }

    private void Configure(string? name, RateLimiterOptions options)
    {
        options.AddFixedWindowLimiter("Fixed", limiterOptions =>
        {
            limiterOptions.Window = TimeSpan.FromSeconds(_options.Value.Window);
            limiterOptions.PermitLimit = _options.Value.PermitLimit;
            limiterOptions.QueueLimit = _options.Value.QueueLimit;
        });
    }

    public void Configure(RateLimiterOptions options) => Configure(Options.DefaultName, options);
}