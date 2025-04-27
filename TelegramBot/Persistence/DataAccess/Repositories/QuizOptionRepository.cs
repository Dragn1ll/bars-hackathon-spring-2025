using Domain.Abstractions.Repositories;
using Domain.Entities;

namespace Persistence.DataAccess.Repositories;

public class QuizOptionRepository(AppDbContext context) : 
    AbstractRepository<QuizOptionEntity>(context), 
    IQuizOptionRepository
{
    public async Task<IEnumerable<QuizOptionEntity?>> GetAllQuizOptionsAsync()
    {
        return await GetAllByFilterAsync(e => e.IsDeleted == false);
    }

    public async Task<IEnumerable<QuizOptionEntity?>> GetDeletedQuizOptionsAsync()
    {
        return await GetAllByFilterAsync(e => e.IsDeleted == true);
    }

    public async Task<IEnumerable<QuizOptionEntity?>> GetQuizOptionsByQuizQuestionIdAsync(Guid quizId)
    {
        return await GetAllByFilterAsync(e => e.QuestionId == quizId);
    }

    public async Task<QuizOptionEntity?> GetQuizOptionsByIdAsync(Guid optionId)
    {
        return await GetByFilterAsync(e => e.OptionId == optionId);
    }

    public async Task<bool> PatchQuizOptionTextAsync(Guid optionId, string newText)
    {
        return await PatchAsync(optionId, e => e.Text = newText);
    }

    public async Task<bool> PatchDeleteStatusAsync(Guid optionId)
    {
        return await PatchAsync(optionId, e => e.IsDeleted = !e.IsDeleted);
    }

    public async Task<bool> DeleteQuizOptionAsync(Guid optionId)
    {
        return await DeleteAsync(e => e.OptionId == optionId);
    }
}