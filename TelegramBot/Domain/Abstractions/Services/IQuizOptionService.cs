using Domain.Entities;
using Domain.Utils;

namespace Domain.Abstractions.Services;

public interface IQuizOptionService
{
    Task<Result<QuizOptionEntity>> AddQuizOption(QuizOptionEntity quizOption);
    Task<Result> DeleteQuizOption(QuizOptionEntity quizOption);
    Task<Result<List<QuizOptionEntity>>> GetQuizOptions(int quizId);
}