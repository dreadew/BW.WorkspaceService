using Common.Base.DTO;
using WorkspaceService.Domain.DTOs.WorkspacePositions;

namespace WorkspaceService.Domain.Services;

public interface IWorkspacePositionsService
{
    Task CreateAsync(CreatePositionRequest dto,
        CancellationToken cancellationToken = default);
    Task UpdateAsync(UpdatePositionRequest dto,
        CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task RestoreAsync(Guid id, CancellationToken cancellationToken = default);
    Task<PositionDto> GetByIdAsync(Guid id,
        CancellationToken cancellationToken = default);
    Task<IEnumerable<PositionDto>> ListAsync(ListRequest dto, Guid workspaceId,
        CancellationToken cancellationToken = default);
}