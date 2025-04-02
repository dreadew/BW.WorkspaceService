using WorkspaceService.Domain.DTOs.Identity;

namespace WorkspaceService.Domain.Services;

public interface IIdentityServiceClient
{
    Task<(bool, string?)> VerifyAsync(string accessToken,
        CancellationToken cancellationToken = default);

    Task<UserDto?> GetByIdAsync(string id,
        CancellationToken cancellationToken = default);

    Task<List<UserDto>> GetFromArrayAsync(List<string> userIds,
        CancellationToken cancellationToken = default);
}