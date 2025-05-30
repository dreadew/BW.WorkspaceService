using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WorkspaceService.Domain.Entities;
using WorkspaceService.Domain.Exceptions;
using WorkspaceService.Domain.Interfaces;
using WorkspaceService.Domain.Services;

namespace WorkspaceService.Application.Services;

public class ClaimService : IClaimsService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ClaimService> _logger;

    public ClaimService(ILogger<ClaimService> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<bool> CheckUserClaim(string workspaceId, string userId,
        string expectedClaim, CancellationToken token = default)
    {
        var workspaceRepo = _unitOfWork.Repository<Workspace>();
        var workspace = await workspaceRepo
            .FindMany(x => x.Id == workspaceId)
            .FirstOrDefaultAsync(token);
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