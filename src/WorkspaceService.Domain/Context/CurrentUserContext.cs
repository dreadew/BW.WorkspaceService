namespace WorkspaceService.Domain.Context;

public static class CurrentUserContext
{
    private static readonly AsyncLocal<string?> _currentUserId = new AsyncLocal<string?>();

    public static string CurrentUserId
    {
        get => _currentUserId.Value ?? "";
        set => _currentUserId.Value = value;
    }
}