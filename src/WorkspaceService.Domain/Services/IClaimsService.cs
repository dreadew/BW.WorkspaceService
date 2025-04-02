namespace WorkspaceService.Domain.Services;

public interface IClaimsService
{
    Task<bool> CheckUserClaim(string workspaceId, string userId,
        string expectedClaim, CancellationToken token = default);
}