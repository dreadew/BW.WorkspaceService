namespace WorkspaceService.Domain.Services;

public interface IVaultService
{
    Task<T?> GetSecretAsync<T>(string key);
}