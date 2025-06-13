using AutoMapper;
using WorkspaceService.Domain.DTOs.WorkspacePositions;
using WorkspaceService.Domain.Entities;

namespace WorkspaceService.Application.Mapping;

public class WorkspacePositionProfile : Profile
{
    public WorkspacePositionProfile()
    {
        CreateMap<WorkspacePosition, PositionDto>();
        CreateMap<CreatePositionRequest, WorkspacePosition>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.WorkspaceId, 
                opt => opt.MapFrom(src => src.Id));
        CreateMap<UpdatePositionRequest, WorkspacePosition>();
    }
}