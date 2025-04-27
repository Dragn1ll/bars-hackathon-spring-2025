using Domain.Entities;
using Domain.Utils;

namespace Domain.Abstractions.Services;

public interface IQuizQuestionService
{
    Task<Result<QuizQuestionEntity>> CreateQuizQuestion(QuizQuestionEntity quizQuestion);
    Task<Result> DeleteQuizQuestion(int quizQuestionId);
    Task<Result<QuizQuestionEntity>> GetQuizQuestion(int quizQuestionId);
    Task<Result<List<QuizQuestionEntity>>> GetQuizQuestions(int lessonId);
}