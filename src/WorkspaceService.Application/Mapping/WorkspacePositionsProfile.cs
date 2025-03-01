using AutoMapper;
using WorkspaceService.Domain.DTOs.WorkspacePositions;
using WorkspaceService.Domain.Entities;

namespace WorkspaceService.Application.Mapping;

public class WorkspacePositionsProfile : Profile
{
    public WorkspacePositionsProfile()
    {
        CreateMap<WorkspacePositions, PositionDto>();
        CreateMap<CreatePositionRequest, WorkspacePositions>();
        CreateMap<UpdatePositionRequest, WorkspacePositions>();
    }
}