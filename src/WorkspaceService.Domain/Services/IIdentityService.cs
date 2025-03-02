namespace WorkspaceService.Domain.Services;

public interface IIdentityService
{
    Task<bool> VerifyAsync(string accessToken);
}