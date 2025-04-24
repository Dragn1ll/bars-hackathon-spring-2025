using Domain.Entities;

namespace Domain.Abstractions.Services;

public interface IQuizQuestionService
{
    Task<QuizQuestionEntity> CreateQuizQuestion(QuizQuestionEntity quizQuestion);
    Task<bool> DeleteQuizQuestion(int quizQuestionId);
    Task<QuizQuestionEntity> GetQuizQuestion(int quizQuestionId);
    Task<List<QuizQuestionEntity>> GetQuizQuestions(int lessonId);
}