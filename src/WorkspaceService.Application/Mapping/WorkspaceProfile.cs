using AutoMapper;
using Common.Base.DTO.Entity;
using Common.Services.MappingActions;
using WorkspaceService.Domain.DTOs.Workspaces;
using WorkspaceService.Domain.DTOs.WorkspaceUsers;
using WorkspaceService.Domain.Entities;

namespace WorkspaceService.Application.Mapping;

public class WorkspaceProfile : Profile
{
    public WorkspaceProfile()
    {
        CreateMap<Workspace, WorkspaceDto>()
            .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.Users))
            .AfterMap<AddPathAction<Workspace, WorkspaceDto>>();
        CreateMap<CreateWorkspaceRequest, Workspace>();
        CreateMap<UpdateWorkspaceRequest, Workspace>();
        CreateMap<WorkspaceUser, WorkspaceUserDto>()
            .ForMember(dest => dest.Id,
                opt => opt.MapFrom(src => src.UserId));
        CreateMap<BaseDirectoryRequest, WorkspaceDirectory>()
            .ForMember(dest => dest.Id, 
                opt => opt.Ignore());
        CreateMap<WorkspaceDirectoryArtifact, BaseArtifactDto>()
            .AfterMap<AddPathAction<WorkspaceDirectoryArtifact, BaseArtifactDto>>();
    }
}