using Domain.Entities;
using Domain.Models.Dto.Admin;
using Domain.Models.Dto.Bot;
using Domain.Utils;

namespace Domain.Abstractions.Services;

public interface IQuizQuestionService
{
    Task<Result<AdminQuestionResponseDto>> CreateQuizQuestion(CreateQuestionDto quizQuestion);
    Task<Result> DeleteQuizQuestion(Guid quizQuestionId);
    Task<Result<BotQuestionResponseDto>> GetQuizQuestionForUser(Guid lessonId, long userId);
    Task<Result<BotQuestionResponseDto>> GetNextQuestionForUser(UserAnswerDtoRequest userAnswerDto);
}