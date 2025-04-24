namespace Domain.Abstractions.Services;

public interface IFileStorageService
{
    Task<bool> UploadFileAsync(string fileName, Stream fileStream, string contentType, CancellationToken cancellationToken = default);
    Task<byte[]> DownloadFileAsync(string fileName, CancellationToken cancellationToken = default);
    Task<bool> UpdateFileAsync(string fileName, Stream fileStream, CancellationToken cancellationToken = default);
    Task<bool> DeleteFileAsync(string fileName, CancellationToken cancellationToken = default);
    Task<bool> FileExistsAsync(string fileName, CancellationToken cancellationToken = default);
    Task<string?> GetFileUrlAsync(string fileName, CancellationToken cancellationToken = default);
}