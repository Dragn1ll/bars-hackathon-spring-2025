using System.Linq.Expressions;
using Domain.Abstractions.Repositories;
using Domain.Abstractions.Services;
using Domain.Entities;
using Domain.Models.Dto.Admin;
using Domain.Models.Enums;
using Domain.Utils;

namespace Application.Services;

public class QuizOptionService(IUnitOfWork unitOfWork, Mapper mapper) : IQuizOptionService
{
    public async Task<Result<AdminQuestionOptionResponseDto>> AddQuizOption(CreateQuestionOptionDto quizOption)
    {
        try
        {
            return await unitOfWork.QuizOptions.AddAsync(
                mapper.Map<CreateQuestionOptionDto, QuizOptionEntity>(quizOption))
                ? Result<AdminQuestionOptionResponseDto>.Success(
                    mapper.Map<CreateQuestionOptionDto, AdminQuestionOptionResponseDto>(quizOption))
                : Result<AdminQuestionOptionResponseDto>.Failure(
                    new Error(ErrorType.ServerError, "Cannot add quiz option"));
        }
        catch (Exception exception)
        {
            return Result<AdminQuestionOptionResponseDto>.Failure(
                new Error(ErrorType.ServerError, exception.Message));
        }
    }

    public async Task<Result> DeleteQuizOption(int quizOptionId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> DeleteQuizOption(Guid quizOptionId)
    {
        try
        {
            return await unitOfWork.QuizOptions.DeleteAsync(qo => qo.OptionId == quizOptionId)
                ? Result.Success()
                : Result.Failure(new Error(ErrorType.ServerError, "Cannot delete quiz option"));
        }
        catch (Exception exception)
        {
            return Result.Failure(new Error(ErrorType.ServerError, exception.Message));
        }
    }
}