using AutoMapper;
using WorkspaceService.Application.MappingActions;
using WorkspaceService.Domain.DTOs.Workspaces;
using WorkspaceService.Domain.Entities;

namespace WorkspaceService.Application.Mapping;

public class WorkspacesProfile : Profile
{
    public WorkspacesProfile()
    {
        CreateMap<Workspaces, WorkspaceDto>()
            .AfterMap<WorkspaceMappingAction>();
        CreateMap<CreateWorkspaceRequest, Workspaces>();
        CreateMap<UpdateWorkspaceRequest, Workspaces>();
    }
}