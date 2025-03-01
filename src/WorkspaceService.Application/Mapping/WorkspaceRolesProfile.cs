using AutoMapper;
using WorkspaceService.Domain.DTOs.WorkspaceRoles;
using WorkspaceService.Domain.Entities;

namespace WorkspaceService.Application.Mapping;

public class WorkspaceRolesProfile : Profile
{
    public WorkspaceRolesProfile()
    {
        CreateMap<CreateRoleRequest, WorkspaceRoles>();
        CreateMap<UpdateRoleRequest, WorkspaceRoles>();
        CreateMap<WorkspaceRoles, RoleDto>();
    }
}