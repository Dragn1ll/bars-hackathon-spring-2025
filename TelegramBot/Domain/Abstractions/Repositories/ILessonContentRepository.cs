using Domain.Entities;

namespace Domain.Abstractions.Repositories;

public interface ILessonContentRepository : IRepository<LessonContentEntity>
{
    Task<IEnumerable<LessonContentEntity?>> GetAllLessonContentsAsync();
    Task<IEnumerable<LessonContentEntity?>> GetDeletedLessonContentsAsync();
    Task<IEnumerable<LessonContentEntity?>> GetLessonContentsByLessonIdAsync(int lessonId);
    Task<IEnumerable<LessonContentEntity?>> GetLessonContentsByTypeAsync(int typeId);
}