using Domain.Entities;

namespace Domain.Abstractions.Repositories;

public interface IQuizOptionRepository : IRepository<QuizOptionEntity>
{
    Task<IEnumerable<QuizOptionEntity?>> GetAllQuizOptionsAsync();
    Task<IEnumerable<QuizOptionEntity?>> GetDeletedQuizOptionsAsync();
    Task<IEnumerable<QuizOptionEntity?>> GetQuizOptionsByQuizQuestionIdAsync(int quizId);
    Task<QuizOptionEntity?> GetQuizOptionsByIdAsync(int optionId);
    Task<bool> PatchQuizOptionTextAsync(int optionId, string newText);
    Task<bool> PatchDeleteStatusAsync(int optionId);
    Task<bool> DeleteQuizOptionAsync(int optionId);
}