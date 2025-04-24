using Domain.Entities;

namespace Domain.Abstractions.Services;

public interface ILessonContentService
{
    Task<LessonContentEntity> AddLessonContent(LessonContentEntity lessonContent);
    Task<bool> RemoveLessonContent(int lessonContentId);
    Task<LessonContentEntity> GetLessonContent(int lessonContentId);
    Task<List<LessonContentEntity>> GetAllLessonContent(int lessonId);
}