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

    public async Task<IEnumerable<QuizOptionEntity?>> GetQuizOptionsByQuizQuestionIdAsync(int quizId)
    {
        return await GetAllByFilterAsync(e => e.QuestionId == quizId);
    }

    public async Task<QuizOptionEntity?> GetQuizOptionsByIdAsync(int optionId)
    {
        return await GetByFilterAsync(e => e.Id == optionId);
    }

    public async Task<bool> PatchQuizOptionTextAsync(int optionId, string newText)
    {
        return await PatchAsync(optionId, e => e.Text = newText);
    }

    public async Task<bool> PatchDeleteStatusAsync(int optionId)
    {
        return await PatchAsync(optionId, e => e.IsDeleted = !e.IsDeleted);
    }

    public async Task<bool> DeleteQuizOptionAsync(int optionId)
    {
        return await DeleteAsync(e => e.Id == optionId);
    }
}