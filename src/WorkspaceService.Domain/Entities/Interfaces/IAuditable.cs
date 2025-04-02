namespace DefaultNamespace;

public interface IAuditable
{
    public DateTime CreatedAt { get; }
    public DateTime? ModifiedAt { get; }
    public string? ChangedBy { get; }
}