using Domain.Abstractions.Repositories;
using Domain.Entities;

namespace Persistence.DataAccess.Repositories;

public class LessonRepository(AppDbContext context) : 
    AbstractRepository<LessonEntity>(context), 
    ILessonRepository
{
    public async Task<IEnumerable<LessonEntity?>> GetAllLessonsAsync()
    {
        return await GetAllByFilterAsync(e => e.IsDeleted == false);
    }

    public async Task<IEnumerable<LessonEntity?>> GetDeletedLessonsAsync()
    {
        return await GetAllByFilterAsync(e => e.IsDeleted == true);
    }

    public async Task<IEnumerable<LessonEntity?>> GetLessonsByModuleIdAsync(int moduleId)
    {
        return await GetAllByFilterAsync(e => e.ModuleId == moduleId);
    }

    public async Task<IEnumerable<LessonEntity?>> GetLessonsByTitleAsync(string title)
    {
        return await GetAllByFilterAsync(e => e.Title
            .Contains(title, StringComparison.InvariantCultureIgnoreCase));
    }

    public async Task<LessonEntity?> GetLessonByIdAsync(int lessonId)
    {
        return await GetByFilterAsync(e => e.Id == lessonId);
    }

    public async Task<bool> PatchLessonTitleAsync(int lessonId, string newTitle)
    {
        return await PatchAsync(lessonId, e => e.Title = newTitle);
    }

    public async Task<bool> PatchDeleteStatusAsync(int lessonId)
    {
        return await PatchAsync(lessonId, e => e.IsDeleted = !e.IsDeleted);
    }

    public async Task<bool> DeleteLessonAsync(int moduleId)
    {
        return await DeleteAsync(e => e.ModuleId == moduleId);
    }
}