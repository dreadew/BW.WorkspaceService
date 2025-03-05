namespace WorkspaceService.Domain.DTOs;

public record class ListRequest(int Limit = 20, int Offset = 0);