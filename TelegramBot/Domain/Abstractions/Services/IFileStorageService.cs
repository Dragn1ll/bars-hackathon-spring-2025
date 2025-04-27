using Domain.Utils;

namespace Domain.Abstractions.Services;

public interface IFileStorageService
{
    Task<Result> UploadFileAsync(string fileName, Stream fileStream, string contentType, CancellationToken cancellationToken = default);
    Task<Result<Stream>> DownloadFileAsync(string fileName, CancellationToken cancellationToken = default);
    Task<Result> DeleteFileAsync(string fileName, CancellationToken cancellationToken = default);
    Task<Result> FileExistsAsync(string fileName, CancellationToken cancellationToken = default);
}