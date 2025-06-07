using Common.Base.DTO.Grpc;
using WorkspaceService.Domain.DTOs.WorkspacePositions;
using WorkspaceService.Domain.DTOs.WorkspaceRoles;

namespace WorkspaceService.Domain.DTOs.WorkspaceUsers;

public class WorkspaceUserDto
{
    public string UserId { get; set; }
    public RoleDto Role { get; set; }
    public PositionDto Position { get; set; }
    public UserDto? User { get; set; }
}