using Microsoft.Extensions.Configuration;
using WorkspaceService.Domain.Options;

namespace WorkspaceService.Infrastructure.Configuration;

public class VaultConfigurationSource : IConfigurationSource
{
    private readonly VaultOptions _options;

    public VaultConfigurationSource(VaultOptions options)
    {
        _options = options;
    }

    public IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        return new VaultConfigurationProvider(_options);
    }
}