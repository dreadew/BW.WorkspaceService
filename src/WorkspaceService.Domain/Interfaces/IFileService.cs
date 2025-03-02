using WorkspaceService.Domain.DTOs.File;

namespace WorkspaceService.Domain.Interfaces;

public interface IFileService
{
    /// <summary>
    /// Загружает файл фото профиля в S3 и возвращает URL.
    /// </summary>
    Task<UploadedFileResponse> UploadProfilePhotoAsync(
        FileUploadDto dto,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Удаляет фото профиля из S3 по идентификатору пользователя.
    /// Реализация может извлекать имя объекта из БД.
    /// </summary>
    Task DeleteProfilePhotoAsync(
        FileDeleteDto dto,
        CancellationToken cancellationToken = default);
}