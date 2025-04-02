namespace WorkspaceService.Domain.Exceptions;

/// <summary>
/// Исключение, возникающее в сервисном слое.
/// Флаг visibleToUser указывает, можно ли показывать сообщение пользователю
/// </summary>
public class ServiceException : Exception
{
    public bool visibleToUser { get; }

    public ServiceException(string message, bool visibleToUser = true)
        : base(message)
    {
        this.visibleToUser = visibleToUser;
    }

    public ServiceException(string message, Exception innerException, bool visibleToUser = true)
        : base(message, innerException)
    {
        this.visibleToUser = visibleToUser;
    }
}