using Common.Base.DTO.Entity;
using Common.Base.DTO.Grpc;
using WorkspaceService.Domain.DTOs.WorkspacePositions;
using WorkspaceService.Domain.DTOs.WorkspaceRoles;

namespace WorkspaceService.Domain.DTOs.WorkspaceUsers;

public class WorkspaceUserDto : BaseDto
{
    public RoleDto Role { get; set; }
    public PositionDto Position { get; set; }
    public UserDto? User { get; set; }
}