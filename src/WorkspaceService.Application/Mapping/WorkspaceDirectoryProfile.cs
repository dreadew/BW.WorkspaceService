using AutoMapper;
using Common.Base.DTO.Entity;
using Common.Services.MappingActions;
using WorkspaceService.Domain.DTOs.WorkspaceDirectory;
using WorkspaceService.Domain.DTOs.WorkspaceDirectoryArtifact;
using WorkspaceService.Domain.Entities;

namespace WorkspaceService.Application.Mapping;

public class WorkspaceDirectoryProfile : Profile
{
    public WorkspaceDirectoryProfile()
    {
        CreateMap<WorkspaceDirectory, BaseDirectoryDto>()
            .ForMember(dest => dest.Artifacts, opt => opt.MapFrom(src => src.Artifacts))
            .ForMember(dest => dest.Children, opt => opt.MapFrom(src => src.Children.Where(c => !c.IsDeleted)));
        CreateMap<WorkspaceDirectoryArtifact, BaseArtifactDto>()
            .AfterMap<AddPathAction<WorkspaceDirectoryArtifact, BaseArtifactDto>>();
    }
}