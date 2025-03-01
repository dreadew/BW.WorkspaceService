using AutoMapper;
using WorkspaceService.Domain.DTOs.WorkspaceDirectory;
using WorkspaceService.Domain.DTOs.Workspaces;
using WorkspaceService.Domain.Entities;

namespace WorkspaceService.Application.Mapping;

public class WorkspaceDirectoryProfile : Profile
{
    public WorkspaceDirectoryProfile()
    {
        CreateMap<WorkspaceDirectory, DirectoryDto>();
        CreateMap<CreateWorkspaceRequest, WorkspaceDirectory>();
        CreateMap<UpdateWorkspaceRequest, WorkspaceDirectory>();
    }
}