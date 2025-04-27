using Domain.Abstractions.Services;
using Domain.Models.Enums;
using Domain.Utils;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace Infrastructure.Storage;

public class MinioService : IFileStorageService
{
    private readonly IMinioClient _minioClient;
    private readonly MinioOptions _minioOptions;

    public MinioService(IOptions<MinioOptions> minioOptions)
    {
        _minioOptions = minioOptions.Value;
        _minioClient = new MinioClient()
            .WithEndpoint(_minioOptions.Endpoint)
            .WithCredentials(_minioOptions.AccessKey, _minioOptions.SecretKey)
            .WithSSL(false)
            .Build();
    }

    public async Task<Result> UploadFileAsync(string fileName, Stream fileStream, string contentType,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var bucketExists = await _minioClient.BucketExistsAsync(
                new BucketExistsArgs().WithBucket(_minioOptions.BucketName), cancellationToken);
            if (!bucketExists)
            {
                await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(_minioOptions.BucketName), cancellationToken);
            }

            fileStream.Position = 0;
            await _minioClient.PutObjectAsync(new PutObjectArgs()
                .WithBucket(_minioOptions.BucketName)
                .WithObject(fileName)
                .WithStreamData(fileStream)
                .WithObjectSize(fileStream.Length)
                .WithContentType(contentType), cancellationToken);

            return Result.Success();
        }
        catch
        {
            return Result.Failure(new Error(ErrorType.ServerError, $"Can not upload file {fileName}"));
        }
    }

    public async Task<Result<Stream>> DownloadFileAsync(string fileName, CancellationToken cancellationToken = default)
    {
        try
        {
            var existResult = await FileExistsAsync(fileName, cancellationToken);
            if (!existResult.IsSuccess)
            {
                return Result<Stream>.Failure(existResult.Error);
            }

            var memoryStream = new MemoryStream();
            await _minioClient.GetObjectAsync(
                new GetObjectArgs()
                    .WithBucket(_minioOptions.BucketName)
                    .WithObject(fileName)
                    .WithCallbackStream(stream => stream.CopyTo(memoryStream)), cancellationToken);

            memoryStream.Seek(0, SeekOrigin.Begin);
            return Result<Stream>.Success(memoryStream);
        }
        catch
        {
            return Result<Stream>.Failure(new Error(ErrorType.ServerError, $"Can not download file {fileName}"));
        }
    }

    public async Task<Result> DeleteFileAsync(string fileName, CancellationToken cancellationToken = default)
    {
        try
        {
            var existResult = await FileExistsAsync(fileName, cancellationToken);
            if (!existResult.IsSuccess)
            {
                return Result<Stream>.Failure(existResult.Error);
            }
            await _minioClient.RemoveObjectAsync(new RemoveObjectArgs()
                .WithBucket(_minioOptions.BucketName)
                .WithObject(fileName), cancellationToken);

            return Result.Success();
        }
        catch (Exception exception)
        {
            return Result.Failure(new Error(ErrorType.ServerError, "Could not delete file"));
        }
    }

    public async Task<Result> FileExistsAsync(string fileName, CancellationToken cancellationToken = default)
    {
        try
        {

            await _minioClient.StatObjectAsync(new StatObjectArgs()
                .WithBucket(_minioOptions.BucketName)
                .WithObject(fileName), cancellationToken);

            return Result<bool>.Success(true);
        }
        catch (Minio.Exceptions.ObjectNotFoundException)
        {
            return Result<bool>.Success(false);
        }
        catch (Exception exception)
        {
            return Result<bool>.Failure(new Error(ErrorType.ServerError, "File not found."));
        }
    }
}