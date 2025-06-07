using Grpc.Core;
using Microsoft.Extensions.Logging;
using WorkspaceService.Domain.Services;
using WorkspaceService.Grpc.Protos;

namespace WorkspaceService.Grpc.Services;

public class WorkspaceService : Protos.WorkspaceService.WorkspaceServiceBase
{
    private readonly IWorkspaceService _workspaceService;
    private readonly IWorkspaceRolesService _workspaceRolesService;
    private readonly ILogger<WorkspaceService> _logger;

    public WorkspaceService(IWorkspaceService workspaceService,
        IWorkspaceRolesService workspaceRolesService, ILogger<WorkspaceService> logger)
    {
        _workspaceService = workspaceService;
        _workspaceRolesService = workspaceRolesService;
        _logger = logger;
    }

    public override async Task<GetClaimsByUserResponse> GetClaimsByUser(
        GetClaimsByUserRequest request, ServerCallContext context)
    {
        var workspace = await _workspaceService.GetByIdAsync(Guid.Parse(request.WorkspaceId), context.CancellationToken);
        if (workspace == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Workspace not found"));
        }
        
        var user = workspace.Users
            .FirstOrDefault(x => x.UserId == request.UserId);
        if (user == null)
        {
            throw new RpcException(new Status(StatusCode.NotFound, "User not found"));
        }

        var claims = user.Role.Claims.Select(x => new RoleClaim()
        {
            Name = x.Value
        }).ToList();

        return new GetClaimsByUserResponse()
        {
            Claims = { claims }
        };
    }
}