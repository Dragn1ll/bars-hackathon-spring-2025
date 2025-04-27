using Domain.Abstractions.Repositories;
using Domain.Entities;

namespace Persistence.DataAccess.Repositories;

public class UserCompletedLessonRepository(AppDbContext context) : 
    AbstractRepository<UserCompletedLessonEntity>(context), 
    IUserCompletedLessonRepository
{
    public async Task<IEnumerable<UserCompletedLessonEntity?>> GetAllUserCompletedLessonsAsync()
    {
        return await GetAllByFilterAsync(e => true);
    }

    public async Task<IEnumerable<UserCompletedLessonEntity?>> GetUserCompletedLessonsByUerIdAsync(int userId)
    {
        return await GetAllByFilterAsync(e => e.UserId == userId);
    }

    public async Task<IEnumerable<UserCompletedLessonEntity?>> GetUserCompletedLessonsByLessonIdAsync(Guid lessonId)
    {
        return await GetAllByFilterAsync(e => e.LessonId == lessonId);
    }

    public async Task<UserCompletedLessonEntity?> GetUserCompletedLessonByIdAsync(Guid completeId)
    {
        return await GetByFilterAsync(e => e.Id == completeId);
    }

    public async Task<bool> PatchUserCompletedLessonStatusAsync(Guid completeId, bool newStatus)
    {
        return await PatchAsync(completeId, e => e.IsSuccessful = newStatus);
    }

    public async Task<bool> DeleteUserCompletedLessonAsync(Guid completeId)
    {
        return await DeleteAsync(e => e.Id == completeId);
    }
}