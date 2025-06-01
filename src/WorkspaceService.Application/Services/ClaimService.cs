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
        var claimsRepo = _unitOfWork.Repository<WorkspaceRoleClaim>();
        var workspaceRepo = _unitOfWork.Repository<Workspace>();
        var workspace = await workspaceRepo
            .FindMany(x => x.Id == Guid.Parse(workspaceId))
            .FirstOrDefaultAsync(token);
        if (workspace == null)
        {
            throw new ForbiddenException(workspaceId.ToString(), expectedClaim);
        }

        var user = workspace.Users.FirstOrDefault(x => x.UserId == Guid.Parse(userId));
        if (user == null)
        {
            throw new ForbiddenException(workspaceId.ToString(), expectedClaim);
        }
        
        if (!(await claimsRepo.FindMany(x => x.RoleId == user.RoleId && 
                                             x.Value == expectedClaim).AnyAsync(token)))
        {
            throw new ForbiddenException(workspaceId.ToString(), expectedClaim);
        }

        return true;
    }
}