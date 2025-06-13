using AutoMapper;
using Microsoft.EntityFrameworkCore.Design.Internal;
using WorkspaceService.Domain.DTOs.WorkspaceRoles;
using WorkspaceService.Domain.Entities;

namespace WorkspaceService.Application.Mapping;

public class WorkspaceRoleProfile : Profile
{
    public WorkspaceRoleProfile()
    {
        CreateMap<CreateRoleRequest, WorkspaceRole>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.WorkspaceId, 
                opt => opt.MapFrom(src => src.Id));
        CreateMap<UpdateRoleRequest, WorkspaceRole>();
        CreateMap<WorkspaceRole, RoleDto>();
    }
}