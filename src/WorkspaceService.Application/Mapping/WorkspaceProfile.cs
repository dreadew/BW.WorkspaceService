using AutoMapper;
using WorkspaceService.Application.MappingActions;
using WorkspaceService.Domain.DTOs.Workspaces;
using WorkspaceService.Domain.Entities;

namespace WorkspaceService.Application.Mapping;

public class WorkspaceProfile : Profile
{
    public WorkspaceProfile()
    {
        CreateMap<Workspace, WorkspaceDto>()
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles))
            .ForMember(dest => dest.Positions, opt => opt.MapFrom(src => src.Positions))
            .ForMember(dest => dest.Directories, opt => opt.MapFrom(src => src.Directories))
            .ForMember(dest => dest.Users, opt => opt.MapFrom(src => src.Users))
            .AfterMap<WorkspaceMappingAction>();
        CreateMap<CreateWorkspaceRequest, Workspace>();
        CreateMap<UpdateWorkspaceRequest, Workspace>();
    }
}