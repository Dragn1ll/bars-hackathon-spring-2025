using Domain.Abstractions.Repositories;
using Domain.Entities;

namespace Persistence.DataAccess.Repositories;

public class LessonContentRepository(AppDbContext context) : 
    AbstractRepository<LessonContentEntity>(context),
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
}