using Domain.Entities;

namespace Domain.Abstractions.Repositories;

public interface IUserCompletedLessonRepository : IRepository<UserCompletedLessonEntity>
{
    Task<IEnumerable<UserCompletedLessonEntity?>> GetAllUserCompletedLessonsAsync();
    Task<IEnumerable<UserCompletedLessonEntity?>> GetUserCompletedLessonsByUerIdAsync(int userId);
    Task<IEnumerable<UserCompletedLessonEntity?>> GetUserCompletedLessonsByLessonIdAsync(int lessonId);
    Task<UserCompletedLessonEntity?> GetUserCompletedLessonByIdAsync(int completeId);
    Task<bool> PatchUserCompletedLessonScoreAsync(int completeId, double newScore);
    Task<bool> PatchUserCompletedLessonStatusAsync(int completeId, bool newStatus);
    Task<bool> DeleteUserCompletedLessonAsync(int completeId);
}