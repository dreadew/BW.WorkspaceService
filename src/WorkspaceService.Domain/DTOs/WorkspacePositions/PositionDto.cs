namespace WorkspaceService.Domain.DTOs.WorkspacePositions;

public class PositionDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}