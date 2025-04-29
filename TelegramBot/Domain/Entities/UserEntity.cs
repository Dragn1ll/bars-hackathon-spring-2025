using Domain.Models.Enums;

namespace Domain.Entities;

public class UserEntity
{
    public long UserId { get; set; }
    public string PhoneNumber { get; set; }
    public ICollection<UserCompletedLessonEntity> CompletedLessons { get; set; }
    public ICollection<UserAnsweredQuestionEntity> Answers { get; set; }
}