using Domain.Entities;

namespace Domain.Abstractions.Repositories;

public interface IQuizQuestionRepository : IRepository<QuizQuestionEntity>
{
    Task<QuizQuestionEntity?> GetQuestionWithOptions(Guid questionId); 
}