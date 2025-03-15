using WorkspaceService.Domain.DTOs.Identity;

namespace WorkspaceService.Domain.Services;

public interface IIdentityService
{
    Task<bool> VerifyAsync(string accessToken,
        CancellationToken cancellationToken = default);

    Task<UserDto?> GetByIdAsync(string id,
        CancellationToken cancellationToken = default);

    Task<List<UserDto>> GetFromArrayAsync(List<string> userIds,
        CancellationToken cancellationToken = default);
}