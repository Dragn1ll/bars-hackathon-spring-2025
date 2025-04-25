using System.Runtime.InteropServices.JavaScript;
using System.Text;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;

namespace Infrastructure.Storage;

public class MinioService
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
    
    public async Task CreateDocument(Guid documentId)
    {
        try
        {
            var bucketExists = await _minioClient.BucketExistsAsync(
                new BucketExistsArgs().WithBucket(_minioOptions.BucketName));
            if (!bucketExists)
            {
                await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(_minioOptions.BucketName));
            }

            var objectName = $"{documentId}.txt";

            using var stream = new MemoryStream("Hello World!"u8.ToArray());
            await _minioClient.PutObjectAsync(new PutObjectArgs()
                .WithBucket(_minioOptions.BucketName)
                .WithObject(objectName)
                .WithStreamData(stream)
                .WithObjectSize(stream.Length)
                .WithContentType("text/plain"));

            return Result.Success();
        }
        catch (Exception exception)
        {
            return Result.Failure(new JSType.Error(ErrorType.ServerError, exception.Message));
        }
    }

    public async Task<Result<string>> PullDocument(Guid documentId)
    {
        try
        {
            var objectName = $"{documentId}.txt";
            var content = string.Empty;

            await _minioClient.GetObjectAsync(new GetObjectArgs()
                .WithBucket(_minioOptions.BucketName)
                .WithObject(objectName)
                .WithCallbackStream(stream =>
                {
                    using var reader = new StreamReader(stream);
                    content = reader.ReadToEnd();
                }));

            return Result<string>.Success(content);
        }
        catch (Exception exception)
        {
            return Result<string>.Failure(new JSType.Error(ErrorType.ServerError, exception.Message));
        }
    }

    public async Task<Result> PushDocument(Guid documentId, string content)
    {
        try
        {
            var objectName = $"{documentId}.txt";

            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            await _minioClient.PutObjectAsync(new PutObjectArgs()
                .WithBucket(_minioOptions.BucketName)
                .WithObject(objectName)
                .WithStreamData(stream)
                .WithObjectSize(stream.Length)
                .WithContentType("text/plain"));

            return Result.Success();
        }
        catch (Exception exception)
        {
            return Result.Failure(new JSType.Error(ErrorType.ServerError, exception.Message));
        }
    }
    
    public async Task<Result> DeleteDocument(Guid documentId)
    {
        try
        {
            var objectName = $"{documentId}.txt";

            await _minioClient.RemoveObjectAsync(new RemoveObjectArgs()
                .WithBucket(_minioOptions.BucketName)
                .WithObject(objectName));

            return Result.Success();
        }
        catch (Exception exception)
        {
            return Result.Failure(new JSType.Error(ErrorType.ServerError, exception.Message));
        }
    }
    
    public async Task<Result<bool>> DocumentExists(Guid documentId)
    {
        try
        {
            var objectName = $"{documentId}.txt";

            await _minioClient.StatObjectAsync(new StatObjectArgs()
                .WithBucket(_minioOptions.BucketName)
                .WithObject(objectName));

            return Result<bool>.Success(true);
        }
        catch (Minio.Exceptions.ObjectNotFoundException)
        {
            return Result<bool>.Success(false);
        }
        catch (Exception exception)
        {
            return Result<bool>.Failure(new JSType.Error(ErrorType.ServerError, exception.Message));
        }
    }
}