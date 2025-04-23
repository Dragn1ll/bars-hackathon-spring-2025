using Domain.Entities;

namespace Domain.Abstractions.Repositories;

public interface ILessonRepository : IRepository<LessonEntity>
{
    Task<IEnumerable<LessonEntity?>> GetAllLessonsAsync();
    Task<IEnumerable<LessonEntity?>> GetDeletedLessonsAsync();
    Task<IEnumerable<LessonEntity?>> GetLessonsByModuleIdAsync(int moduleId);
    Task<IEnumerable<LessonEntity?>> GetLessonsByTitleAsync(string title);
    Task<IEnumerable<LessonEntity?>> GetLessonsByLessonTypeIdAsync(int lessonTypeId);
    Task<LessonEntity?> GetLessonByIdAsync(int lessonId);
    Task<bool> PatchLessonTitleAsync(int lessonId, string newTitle);
    Task<bool> PatchDeleteStatusAsync(int lessonId);
    Task<bool> DeleteLessonAsync(int moduleId);
}