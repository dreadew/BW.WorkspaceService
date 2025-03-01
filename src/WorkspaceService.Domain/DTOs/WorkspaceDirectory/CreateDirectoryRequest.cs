namespace WorkspaceService.Domain.DTOs.WorkspaceDirectory;

public record class CreateDirectoryRequest(string Name,
    string WorkspaceId);