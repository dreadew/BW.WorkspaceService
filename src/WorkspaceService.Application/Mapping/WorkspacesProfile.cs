using AutoMapper;
using WorkspaceService.Domain.DTOs.Workspaces;
using WorkspaceService.Domain.Entities;

namespace WorkspaceService.Application.Mapping;

public class WorkspacesProfile : Profile
{
    public WorkspacesProfile()
    {
        CreateMap<Workspaces, WorkspaceDto>();
        CreateMap<CreateWorkspaceRequest, Workspaces>();
        CreateMap<UpdateWorkspaceRequest, Workspaces>();
    }
}