using Domain.Entities;
using Domain.Utils;

namespace Domain.Abstractions.Services;

public interface IUserCompletedLessonService
{
    Task<Result<UserCompletedLessonEntity>> AddUserCompletedLesson(UserCompletedLessonEntity userCompletedLesson);
    Task<Result> DeleteUserCompletedLesson(int userCompletedLessonId);
}