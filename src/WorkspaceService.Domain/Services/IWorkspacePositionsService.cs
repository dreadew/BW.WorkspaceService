using WorkspaceService.Domain.DTOs;
using WorkspaceService.Domain.DTOs.WorkspacePositions;

namespace WorkspaceService.Domain.Services;

public interface IWorkspacePositionsService
{
    Task CreateAsync(CreatePositionRequest dto,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(UpdatePositionRequest dto,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(string id, CancellationToken cancellationToken = default);

    Task<PositionDto> GetByIdAsync(string id,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<PositionDto>> ListAsync(ListRequest dto, string workspaceId,
        CancellationToken cancellationToken = default);
}