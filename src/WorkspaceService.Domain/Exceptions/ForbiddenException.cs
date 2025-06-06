using WorkspaceService.Domain.Constants;

namespace WorkspaceService.Domain.Exceptions;

public class ForbiddenException : Exception
{
    public string ResourceKey { get; } = ExceptionResourceKeys.Forbidden;
    public string Target { get; }
    public string ExpectedClaim { get; }

    public ForbiddenException(string target, string expectedClaim)
    {
        Target = target;
        ExpectedClaim = expectedClaim;
    }
}