namespace WorkspaceService.Domain.Exceptions;

public class ForbiddenException : Exception
{
    private string Target { get; set; }
    private string ExpectedClaim { get; set; }
    public new string Message => $"У вас нет возможности '{ExpectedClaim}' для доступа к '{Target}'";

    public ForbiddenException(string target, string expectedClaim)
    {
        Target = target;
        ExpectedClaim = expectedClaim;
    }
}