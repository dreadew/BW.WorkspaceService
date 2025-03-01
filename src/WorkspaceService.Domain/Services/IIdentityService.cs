namespace WorkspaceService.Domain.Services;

public class IIdentityService
{
    Task<bool> VerifyAsync(string accessToken);
}