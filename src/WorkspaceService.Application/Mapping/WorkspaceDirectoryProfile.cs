using AutoMapper;
using WorkspaceService.Domain.DTOs.WorkspaceDirectory;
using WorkspaceService.Domain.DTOs.WorkspaceDirectoryArtifact;
using WorkspaceService.Domain.DTOs.Workspaces;
using WorkspaceService.Domain.Entities;

namespace WorkspaceService.Application.Mapping;

public class WorkspaceDirectoryProfile : Profile
{
    public WorkspaceDirectoryProfile()
    {
        CreateMap<WorkspaceDirectory, DirectoryDto>()
            .ForMember(dest => dest.Artifacts, opt => opt.MapFrom(src => src.Artifacts));
        CreateMap<WorkspaceDirectoryArtifact, ArtifactDto>();
        CreateMap<CreateDirectoryRequest, WorkspaceDirectory>();
        CreateMap<UpdateDirectoryRequest, WorkspaceDirectory>();
    }
}