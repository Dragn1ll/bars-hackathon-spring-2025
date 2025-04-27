using Domain.Models.Dto.Admin;
using Domain.Models.Dto.Bot;
using Domain.Utils;

namespace Domain.Abstractions.Services;

public interface IQuizQuestionService
{
    Task<Result<AdminQuestionResponseDto>> CreateQuizQuestion(CreateQuestionDto quizQuestion);
    Task<Result> DeleteQuizQuestion(int quizQuestionId);
    Task<Result<BotQuestionResponseDto>> GetQuizQuestionForUser(int lessonId, long userId);
    Task<Result<BotQuestionResponseDto>> GetNextQuestionForUser(UserAnswerDtoRequest userAnswerDto);
}