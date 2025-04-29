using Domain.Abstractions.Repositories;
using Domain.Abstractions.Services;
using Domain.Entities;
using Domain.Models.Dto.Admin;
using Domain.Models.Dto.Bot;
using Domain.Models.Enums;
using Domain.Utils;

namespace Application.Services;

public class QuizQuestionService(IUnitOfWork unitOfWork, Mapper mapper) : IQuizQuestionService
{
    public async Task<Result<AdminQuestionResponseDto>> CreateQuizQuestion(CreateQuestionDto quizQuestion)
    {
        try
        {
            var entity = new QuizQuestionEntity
            {
                LessonId = quizQuestion.LessonId,
                QuestionId = Guid.NewGuid(),
                QuestionText = quizQuestion.QuestionText,
                QuizOptions = quizQuestion.Answers.Select(qo => new QuizOptionEntity
                {
                    OptionId = Guid.NewGuid(),
                    Text = qo.Answer,
                    IsCorrect = qo.IsCorrect
                }).ToList()
            };

            foreach (var option in entity.QuizOptions)
            {
                option.QuestionId = entity.QuestionId;
            }
            
            var result = await unitOfWork.QuizQuestions.AddAsync(entity);
            await unitOfWork.SaveChangesAsync();
            
            return result
                ? Result<AdminQuestionResponseDto>.Success(new AdminQuestionResponseDto(entity.QuestionId, 
                    entity.QuestionText, entity.QuizOptions
                        .Select(qo => new AdminQuestionOptionResponseDto(qo.OptionId, qo.Text, qo.IsCorrect))
                        .ToList()))
                : Result<AdminQuestionResponseDto>.Failure(
                    new Error(ErrorType.ServerError, "Could not create quiz question."));
        }
        catch (Exception exception)
        {
            return Result<AdminQuestionResponseDto>.Failure(
                new Error(ErrorType.ServerError, exception.Message));
        }
    }

    public async Task<Result> DeleteQuizQuestion(Guid quizQuestionId)
    {
        try
        {
            if (await unitOfWork.QuizQuestions
                    .GetByFilterAsync(q => q.QuestionId == quizQuestionId) == null)
                return Result.Failure(
                    new Error(ErrorType.NotFound, "Quiz question not found."));

            var result = await unitOfWork.QuizQuestions
                .DeleteAsync(q => q.QuestionId == quizQuestionId);
            await unitOfWork.SaveChangesAsync();
            
            return result
                ? Result.Success()
                : Result.Failure(new Error(ErrorType.ServerError, "Could not delete quiz question."));
        }
        catch (Exception exception)
        {
            return Result.Failure(new Error(ErrorType.ServerError, exception.Message));
        }
    }

    public async Task<Result<BotQuestionResponseDto>> GetQuizQuestionForUser(Guid lessonId, long userId)
    {
        try
        {
            if (await unitOfWork.Lessons
                .GetByFilterAsync(l => l.LessonId == lessonId) == null)
                return Result<BotQuestionResponseDto>.Failure(
                    new Error(ErrorType.NotFound, "Lesson not found."));

            var idQuestion = ((await unitOfWork.QuizQuestions
                    .GetByFilterAsync(e => e.LessonId == lessonId))!)
                .QuestionId;
            
            var question = await unitOfWork.QuizQuestions
                .GetQuestionWithOptions(idQuestion);
            
            if (question == null)
                return Result<BotQuestionResponseDto>.Failure(
                    new Error(ErrorType.NotFound, "Question not found."));

            return Result<BotQuestionResponseDto>.Success(new BotQuestionResponseDto
            (
                question.QuestionId, question.QuestionText, 
                question.QuizOptions.Select(qo => new BotAnswerResponseDto(qo.OptionId, qo.Text)).ToList()
            ));
        }
        catch (Exception exception)
        {
            return Result<BotQuestionResponseDto>
                .Failure(new Error(ErrorType.ServerError, exception.Message));
        }
    }

    public async Task<Result<BotQuestionResponseDto>> GetNextQuestionForUser(UserAnswerDtoRequest userAnswerDto)
    {
        try
        {
            var question = await unitOfWork.QuizQuestions.
                GetByFilterAsync(q => q.QuestionId == userAnswerDto.QuestionId);
            
            if (question == null)
                return Result<BotQuestionResponseDto>.Failure(
                    new Error(ErrorType.NotFound, "Question not found."));

            await unitOfWork.AnsweredQuestionsRepository.AddAsync(new UserAnsweredQuestionEntity
            {
                UserId = userAnswerDto.UserId,
                QuestionId = userAnswerDto.QuestionId,
                IsRight = question.QuizOptions
                    .FirstOrDefault(qo => qo.QuestionId == userAnswerDto.QuestionId && qo.IsCorrect)!
                    .OptionId == userAnswerDto.AnswerId
            });
            
            var nextQuestion = await unitOfWork.QuizQuestions
                .GetByFilterAsync(q => q.QuestionId > userAnswerDto.QuestionId);
            
            var nextQuestionWithOptions = await unitOfWork.QuizQuestions
                .GetQuestionWithOptions(userAnswerDto.QuestionId); 
            
            return Result<BotQuestionResponseDto>.Success(new BotQuestionResponseDto(nextQuestionWithOptions!.QuestionId, 
                nextQuestionWithOptions.QuestionText, nextQuestionWithOptions.QuizOptions
                    .Select(qo => new BotAnswerResponseDto(qo.OptionId, qo.Text)).ToList()));
        }
        catch (Exception exception)
        {
            return Result<BotQuestionResponseDto>
                .Failure(new Error(ErrorType.ServerError, exception.Message));
        }
    }
}