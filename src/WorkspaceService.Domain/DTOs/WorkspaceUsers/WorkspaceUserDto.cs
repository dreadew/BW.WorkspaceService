using WorkspaceService.Domain.DTOs.Identity;
using WorkspaceService.Domain.DTOs.WorkspacePositions;
using WorkspaceService.Domain.DTOs.WorkspaceRoles;

namespace WorkspaceService.Domain.DTOs.WorkspaceUsers;

public record class WorkspaceUserDto(
    string UserId,
    RoleDto Role,
    PositionDto Position)
{
    public UserDto? User { get; set; }
};