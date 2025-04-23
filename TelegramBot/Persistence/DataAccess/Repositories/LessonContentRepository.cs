using Domain.Abstractions.Repositories;
using Domain.Entities;

namespace Persistence.DataAccess.Repositories;

public class LessonContentRepository(AppDbContext context) : AbstractRepository<LessonContentEntity>(context),
    ILessonContentRepository
{
    public async Task<IEnumerable<LessonContentEntity?>> GetAllLessonContentsAsync()
    {
        return await GetAllByFilterAsync(e => e.IsDeleted == false);
    }

    public async Task<IEnumerable<LessonContentEntity?>> GetDeletedLessonContentsAsync()
    {
        return await GetAllByFilterAsync(e => e.IsDeleted == true);
    }

    public async Task<IEnumerable<LessonContentEntity?>> GetLessonContentsByLessonIdAsync(int lessonId)
    {
        return await GetAllByFilterAsync(e => e.LessonId == lessonId);
    }

    public async Task<IEnumerable<LessonContentEntity?>> GetLessonContentsByTypeAsync(int typeId)
    {
        return await GetAllByFilterAsync(e => e.LessonContentTypeId == typeId);
    }

    public async Task<LessonContentEntity?> GetLessonContentsByIdAsync(int contentId)
    {
        return await GetByFilterAsync(e => e.Id == contentId);
    }

    public async Task<bool> PatchLessonContentAsync(int contentId, string newContent)
    {
        return await PatchAsync(contentId, e => e.Content = newContent);
    }

    public async Task<bool> PatchDeleteStatusAsync(int contentId)
    {
        return await PatchAsync(contentId, e => e.IsDeleted = true);
    }

    public async Task<bool> DeleteLessonContentAsync(int contentId)
    {
        return await DeleteAsync(e => e.Id == contentId);
    }
}