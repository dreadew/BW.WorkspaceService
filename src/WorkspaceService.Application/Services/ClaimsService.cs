using Microsoft.Extensions.Logging;
using WorkspaceService.Domain.Entities;
using WorkspaceService.Domain.Exceptions;
using WorkspaceService.Domain.Interfaces;
using WorkspaceService.Domain.Services;

namespace WorkspaceService.Application.Services;

public class ClaimsService : IClaimsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ClaimsService> _logger;

    public ClaimsService(ILogger<ClaimsService> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> CheckUserClaim(string workspaceId, string userId,
        string expectedClaim, CancellationToken token = default)
    {
        var workspaceRepo = _unitOfWork.Repository<Workspaces>();
        var workspace = await workspaceRepo.GetByIdAsync(workspaceId, token);
        if (workspace == null)
        {
            throw new ForbiddenException(workspaceId, expectedClaim);
        }

        var user = workspace.Users.FirstOrDefault(x => x.UserId == userId);
        if (user == null)
        {
            throw new ForbiddenException(workspaceId, expectedClaim);
        }

        if (!user.Role.RoleClaims.Any(x => x.Value == expectedClaim))
        {
            throw new ForbiddenException(workspaceId, expectedClaim);
        }

        return true;
    }
}