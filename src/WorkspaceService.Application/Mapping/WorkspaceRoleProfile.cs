using AutoMapper;
using Microsoft.EntityFrameworkCore.Design.Internal;
using WorkspaceService.Domain.DTOs.WorkspaceRoles;
using WorkspaceService.Domain.Entities;

namespace WorkspaceService.Application.Mapping;

public class WorkspaceRoleProfile : Profile
{
    public WorkspaceRoleProfile()
    {
        CreateMap<CreateRoleRequest, WorkspaceRole>();
        CreateMap<UpdateRoleRequest, WorkspaceRole>();
        CreateMap<WorkspaceRole, RoleDto>()
            .ForMember(dest => dest.Claims, opt => opt.MapFrom(src => src.RoleClaims));
    }
}