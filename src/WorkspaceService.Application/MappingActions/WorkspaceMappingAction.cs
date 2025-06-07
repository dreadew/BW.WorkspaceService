using AutoMapper;
using Common.Base.Options;
using Microsoft.Extensions.Options;
using WorkspaceService.Domain.DTOs.Workspaces;
using WorkspaceService.Domain.Entities;

namespace WorkspaceService.Application.MappingActions;

public class WorkspaceMappingAction : IMappingAction<Workspace, WorkspaceDto>
{
    private readonly IOptions<S3Options>  _options;

    public WorkspaceMappingAction(IOptions<S3Options>  options)
    {
        _options = options;
    }

    public void Process(Workspace source, WorkspaceDto destination, ResolutionContext 
            context)
    {
        var endpoint = _options.Value.Endpoint;
        if (string.IsNullOrEmpty(source.PicturePath))
        {
            destination.PictureUrl = "";
            return;
        }
        destination.PictureUrl = $"{endpoint}/{source.PicturePath}";
    }
}