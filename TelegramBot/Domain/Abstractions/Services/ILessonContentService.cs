using Domain.Entities;
using Domain.Utils;

namespace Domain.Abstractions.Services;

public interface ILessonContentService
{
    Task<Result<LessonContentEntity>> AddLessonContent(LessonContentEntity lessonContent);
    Task<Result> RemoveLessonContent(int lessonContentId);
    Task<Result<LessonContentEntity>> GetLessonContent(int lessonContentId);
    Task<Result<List<LessonContentEntity>>> GetAllLessonContent(int lessonId);
}