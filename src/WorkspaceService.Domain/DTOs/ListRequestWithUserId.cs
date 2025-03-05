namespace WorkspaceService.Domain.DTOs;

public record class ListRequestWithUserId(string UserId) : ListRequest;