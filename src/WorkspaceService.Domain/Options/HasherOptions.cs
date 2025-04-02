namespace WorkspaceService.Domain.Options;

public class HasherOptions
{
    public int SaltSize { get; set; }
    public int KeySize { get; set; }
    public int Iterations { get; set; }
}