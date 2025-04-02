using WorkspaceService.Domain.DTOs.File;

namespace WorkspaceService.Domain.Interfaces;

public interface IFileService
{
    /// <summary>
    /// Загружает файл в S3 и возвращает URI.
    /// </summary>
    Task<UploadedFileResponse> UploadFileAsync(
        FileUploadDto dto,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Удаляет файл из S3 по идентификатору пользователя.
    /// Реализация может извлекать имя объекта из БД.
    /// </summary>
    Task DeleteFileAsync(
        FileDeleteDto dto,
        CancellationToken cancellationToken = default);
}