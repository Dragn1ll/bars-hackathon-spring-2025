using Domain.Entities;

namespace Domain.Abstractions.Repositories;

public interface IQuizQuestionRepository : IRepository<QuizQuestionEntity>
{
    Task<IEnumerable<QuizQuestionEntity?>> GetAllQuizQuestionsAsync();
    Task<IEnumerable<QuizQuestionEntity?>> GetDeletedQuizQuestionsAsync();
    Task<IEnumerable<QuizQuestionEntity?>> GetQuizQuestionsByLessonIdAsync(int lessonId);
    Task<QuizQuestionEntity?> GetQuizQuestionsByIdAsync(int quizId);
    Task<bool> PatchQuizQuestionAsync(int quizId, string newQuestion);
    Task<bool> PatchDeleteStatusAsync(int quizId);
    Task<bool> DeleteQuizQuestionAsync(int quizId);
}