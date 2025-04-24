using Domain.Entities;

namespace Domain.Abstractions.Services;

public interface IQuizOptionService
{
    Task<QuizOptionEntity> AddQuizOption(QuizOptionEntity quizOption);
    Task<bool> DeleteQuizOption(QuizOptionEntity quizOption);
    Task<List<QuizOptionEntity>> GetQuizOptions(int quizId);
}