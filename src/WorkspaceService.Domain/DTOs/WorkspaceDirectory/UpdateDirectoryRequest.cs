namespace WorkspaceService.Domain.DTOs.WorkspaceDirectory;

public record class UpdateDirectoryRequest(string Id,
    string? Name);