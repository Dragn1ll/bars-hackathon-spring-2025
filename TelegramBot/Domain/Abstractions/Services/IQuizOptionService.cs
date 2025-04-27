using Domain.Entities;
using Domain.Models.Dto.Admin;
using Domain.Utils;

namespace Domain.Abstractions.Services;

public interface IQuizOptionService
{
    Task<Result<AdminQuestionOptionResponseDto>> AddQuizOption(CreateQuestionOptionDto quizOption);
    Task<Result> DeleteQuizOption(int quizOptionId);
}