using AutoMapper;
using WorkspaceService.Domain.DTOs.WorkspacePositions;
using WorkspaceService.Domain.Entities;

namespace WorkspaceService.Application.Mapping;

public class WorkspacePositionProfile : Profile
{
    public WorkspacePositionProfile()
    {
        CreateMap<WorkspacePosition, PositionDto>();
        CreateMap<CreatePositionRequest, WorkspacePosition>();
        CreateMap<UpdatePositionRequest, WorkspacePosition>();
    }
}