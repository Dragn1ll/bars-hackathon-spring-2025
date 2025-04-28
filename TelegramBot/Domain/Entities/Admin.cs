using Domain.Models.Enums;

namespace Domain.Entities;

public class Admin
{
    public long UserId { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public bool IsDeleted { get; set; }
    public ICollection<UserCompletedLessonEntity> CompletedLessons { get; set; }
    public ICollection<UserAnsweredQuestionEntity> Answers { get; set; }
}