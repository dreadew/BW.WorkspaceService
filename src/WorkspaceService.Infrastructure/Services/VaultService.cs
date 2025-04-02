using Microsoft.Extensions.Options;
using VaultSharp;
using VaultSharp.V1.AuthMethods.Token;
using WorkspaceService.Domain.Options;
using WorkspaceService.Domain.Services;

namespace WorkspaceService.Infrastructure.Services;

public class VaultService : IVaultService
{
    private readonly IVaultClient _vaultClient;
    private readonly VaultOptions _vaultOptions;

    public VaultService(IOptions<VaultOptions> vaultOptions)
    {
        _vaultOptions = vaultOptions.Value;

        var authMethod = new TokenAuthMethodInfo(_vaultOptions.Token);
        var vaultClientSettings = new VaultClientSettings(_vaultOptions.Address, authMethod);
        _vaultClient = new VaultClient(vaultClientSettings);
    }

    public async Task<T?> GetSecretAsync<T>(string key)
    {
        var secret = await _vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync
            (_vaultOptions.SecretPath, mountPoint: _vaultOptions.MountPath);

        if (secret.Data.Data.TryGetValue(key, out object value))
        {
            return (T)value;
        }

        return default;
    }
}