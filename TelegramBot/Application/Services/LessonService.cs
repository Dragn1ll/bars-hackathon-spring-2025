using System.Linq.Expressions;
using Domain.Abstractions.Repositories;
using Domain.Abstractions.Services;
using Domain.Entities;
using Domain.Models.Dto.Admin;
using Domain.Models.Dto.General;
using Domain.Models.Enums;
using Domain.Utils;

namespace Application.Services;

public class LessonService(IUnitOfWork unitOfWork, Mapper mapper, IFileStorageService storageService) 
    : ILessonService
{
    public async Task<Result<LessonDto>> CreateLesson(CreateLessonDto lesson)
    {
        try
        {
            if (await ThereIsALesson(l => l.Title == lesson.Title))
                return Result<LessonDto>.Failure(
                    new Error(ErrorType.BadRequest, "There is already a lesson with the same title"));
            
            var lessonEntity = mapper.Map<CreateLessonDto, LessonEntity>(lesson);
            lessonEntity.LessonId = Guid.NewGuid();
            
            return await unitOfWork.Lessons.AddAsync(lessonEntity)
                ? Result<LessonDto>.Success(mapper.Map<LessonEntity, LessonDto>(lessonEntity))
                : Result<LessonDto>.Failure(new Error(ErrorType.BadRequest, 
                    "Cannot create lesson"));
        }
        catch (Exception exception)
        {
            return Result<LessonDto>.Failure(new Error(ErrorType.ServerError, exception.Message));
        }
    }

    public async Task<Result<LessonDto>> UpdateLesson(LessonDto lesson)
    {
        try
        {
            if (!await ThereIsALesson(l => l.LessonId == lesson.LessonId))
                return Result<LessonDto>.Failure(
                    new Error(ErrorType.NotFound, "Lesson already not exists"));
            
            return await unitOfWork.Lessons.PatchAsync(lesson.LessonId, l => l.Title = lesson.Title)
                ? Result<LessonDto>.Success(lesson) 
                : Result<LessonDto>.Failure(new Error(ErrorType.ServerError, "Can't update lesson")); 
        }
        catch (Exception exception)
        {
            return Result<LessonDto>.Failure(new Error(ErrorType.ServerError, exception.Message));
        }
    }

    public async Task<Result> DeleteLesson(Guid lessonId)
    {
        try
        {
            if (!await ThereIsALesson(l => l.LessonId == lessonId))
                return Result.Failure(
                    new Error(ErrorType.NotFound, "Lesson already not exists"));
            
            return await unitOfWork.Lessons.DeleteAsync(l => l.LessonId == lessonId)
                ? Result.Success()
                : Result.Failure(new Error(ErrorType.ServerError, "Can't delete lesson"));
        }
        catch (Exception exception)
        {
            return Result.Failure(new Error(ErrorType.ServerError, exception.Message));
        }
    }

    public async Task<Result<List<LessonDto>>> GetAllLessons(Guid moduleId)
    {
        try
        {
            return Result<List<LessonDto>>.Success((await unitOfWork.Lessons
                    .GetAllByFilterAsync(l => l.ModuleId == moduleId))
                .Select(l => mapper.Map<LessonEntity, LessonDto>(l!))
                .ToList());
        }
        catch (Exception exception)
        {
            return Result<List<LessonDto>>.Failure(new Error(ErrorType.ServerError, exception.Message));
        }
    }

    public async Task<Result<List<Stream>>> GetAllLessonFiles(Guid lessonId)
    {
        try
        {
            return Result<List<Stream>>.Success((await unitOfWork.LessonContents
                    .GetAllByFilterAsync(l => l.LessonId == lessonId))
                .Select(lc => storageService.DownloadFileAsync(lc.FileName).Result.Value)
                .ToList());
        }
        catch (Exception exception)
        {
            return Result<List<Stream>>.Failure(new Error(ErrorType.ServerError, exception.Message));
        }
    }

    public async Task<Result<LessonDto>> GetLesson(Guid lessonId)
    {
        try
        {
            if (!await ThereIsALesson(l => l.LessonId == lessonId))
                return Result<LessonDto>.Failure(
                    new Error(ErrorType.NotFound, "Lesson already not exists"));
            
            return Result<LessonDto>.Success(mapper.Map<LessonEntity, LessonDto>(await unitOfWork.Lessons
                .GetByFilterAsync(lesson => lesson.LessonId == lessonId)));
        }
        catch
        {
            return Result<LessonDto>.Failure(new Error(ErrorType.ServerError, "Can't get lesson"));>
        }
    }

    public async Task<bool> ThereIsALesson(Expression<Func<LessonEntity, bool>> predicate)
    {
        return await unitOfWork.Lessons.GetByFilterAsync(predicate) != null;
    }
}