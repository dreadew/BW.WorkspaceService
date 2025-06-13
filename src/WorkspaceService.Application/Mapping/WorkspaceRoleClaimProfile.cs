using AutoMapper;
using WorkspaceService.Domain.DTOs.WorkspaceRoleClaims;
using WorkspaceService.Domain.Entities;

namespace WorkspaceService.Application.Mapping;

public class WorkspaceRoleClaimProfile : Profile
{
    public WorkspaceRoleClaimProfile()
    {
        CreateMap<WorkspaceRoleClaim, RoleClaimsDto>();
        CreateMap<CreateRoleClaimsRequest, WorkspaceRoleClaim>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.RoleId, 
                opt => opt.MapFrom(src => src.Id));
        CreateMap<UpdateRoleClaimsRequest, WorkspaceRoleClaim>();
    }
}