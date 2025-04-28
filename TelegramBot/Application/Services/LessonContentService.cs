using System.Linq.Expressions;
using Domain.Abstractions.Repositories;
using Domain.Abstractions.Services;
using Domain.Entities;
using Domain.Models.Dto.Admin;
using Domain.Models.Enums;
using Domain.Utils;

namespace Application.Services;

public class LessonContentService(IUnitOfWork unitOfWork, Mapper mapper) : ILessonContentService
{
    public async Task<Result<AdminLessonContentResponseDto>> AddLessonContent(CreateLessonContentDto lessonContent)
    {
        try
        {
            if (await ThereIsALessonContent(lc => lc.LessonId == lessonContent.LessonId 
                                            && lc.FileName == lessonContent.FileName))
                return Result<AdminLessonContentResponseDto>.Failure(
                    new Error(ErrorType.BadRequest, "File already exists"));

            var entity = mapper.Map<CreateLessonContentDto, LessonContentEntity>(lessonContent);
            entity.LessonContentId = Guid.NewGuid();

            var result = await unitOfWork.LessonContents.AddAsync(entity);
            await unitOfWork.SaveChangesAsync();
            
            return result
                ? Result<AdminLessonContentResponseDto>.Success(
                    mapper.Map<LessonContentEntity, AdminLessonContentResponseDto>(entity))
                : Result<AdminLessonContentResponseDto>.Failure(
                    new Error(ErrorType.ServerError, "Can't add lesson content"));
        }
        catch (Exception exception)
        {
            return Result<AdminLessonContentResponseDto>
                .Failure(new Error(ErrorType.ServerError, exception.Message));
        }
    }

    public async Task<Result> RemoveLessonContent(Guid lessonContentId)
    {
        try
        {
            if (!await ThereIsALessonContent(lc => lc.LessonContentId == lessonContentId))
                return Result<AdminLessonContentResponseDto>.Failure(
                    new Error(ErrorType.NotFound, "File not found"));

            var result = await unitOfWork.LessonContents
                .DeleteAsync(lc => lc.LessonContentId == lessonContentId);
            await unitOfWork.SaveChangesAsync();
            
            return result
                ? Result.Success()
                : Result.Failure(new Error(ErrorType.ServerError, "Can't remove lesson content"));
        }
        catch (Exception exception)
        {
            return Result.Failure(new Error(ErrorType.ServerError, exception.Message));
        }
    }

    public async Task<Result<List<AdminLessonContentResponseDto>>> GetAllLessonContents(Guid lessonId)
    {
        try
        {
            return Result<List<AdminLessonContentResponseDto>>.Success(
                (await unitOfWork.LessonContents.GetAllByFilterAsync(lc => lc.LessonId == lessonId))
                .Select(lc => mapper.Map<LessonContentEntity, AdminLessonContentResponseDto>(lc))
                .ToList());
        }
        catch (Exception exception)
        {
            return Result<List<AdminLessonContentResponseDto>>
                .Failure(new Error(ErrorType.ServerError, exception.Message));
        }
    }

    private async Task<bool> ThereIsALessonContent(Expression<Func<LessonContentEntity, bool>> predicate)
    {
        return await unitOfWork.LessonContents.GetByFilterAsync(predicate) != null;
    }
}