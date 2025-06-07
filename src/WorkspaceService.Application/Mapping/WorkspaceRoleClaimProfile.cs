using AutoMapper;
using WorkspaceService.Domain.DTOs.WorkspaceRoleClaims;
using WorkspaceService.Domain.Entities;

namespace WorkspaceService.Application.Mapping;

public class WorkspaceRoleClaimProfile : Profile
{
    public WorkspaceRoleClaimProfile()
    {
        CreateMap<WorkspaceRoleClaim, RoleClaimsDto>();
        CreateMap<CreateRoleClaimsRequest, WorkspaceRoleClaim>();
        CreateMap<UpdateRoleClaimsRequest, WorkspaceRoleClaim>();
    }
}