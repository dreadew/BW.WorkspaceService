using AutoMapper;
using Microsoft.Extensions.Options;
using WorkspaceService.Domain.DTOs.Identity;
using WorkspaceService.Domain.DTOs.Workspaces;
using WorkspaceService.Domain.Entities;
using WorkspaceService.Domain.Options;

namespace WorkspaceService.Application.MappingActions;

public class WorkspaceMappingAction : IMappingAction<Workspaces, WorkspaceDto>
{
    private readonly IOptions<S3Options>  _options;

    public WorkspaceMappingAction(IOptions<S3Options>  options)
    {
        _options = options;
    }

    public void Process(Workspaces source, WorkspaceDto destination, ResolutionContext 
            context)
    {
        var endpoint = _options.Value.Endpoint;
        destination.PictureUrl = $"{endpoint}/{source.PicturePath}";
    }
}