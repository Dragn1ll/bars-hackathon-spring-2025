using Domain.Entities;

namespace Domain.Abstractions.Services;

public interface IUserCompletedLessonService
{
    Task<UserCompletedLessonEntity> AddUserCompletedLesson(UserCompletedLessonEntity userCompletedLesson);
    Task<bool> DeleteUserCompletedLesson(int userCompletedLessonId);
}