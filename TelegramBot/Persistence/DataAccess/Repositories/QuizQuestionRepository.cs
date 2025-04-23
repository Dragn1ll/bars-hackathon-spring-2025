using Domain.Abstractions.Repositories;
using Domain.Entities;

namespace Persistence.DataAccess.Repositories;

public class QuizQuestionRepository(AppDbContext context) : AbstractRepository<QuizQuestionEntity>(context), 
    IQuizQuestionRepository
{
    public async Task<IEnumerable<QuizQuestionEntity?>> GetAllQuizQuestionsAsync()
    {
        return await GetAllByFilterAsync(e => e.IsDeleted == false);
    }

    public async Task<IEnumerable<QuizQuestionEntity?>> GetDeletedQuizQuestionsAsync()
    {
        return await GetAllByFilterAsync(e => e.IsDeleted == true);
    }

    public async Task<IEnumerable<QuizQuestionEntity?>> GetQuizQuestionsByLessonIdAsync(int lessonId)
    {
        return await GetAllByFilterAsync(e => e.LessonId == lessonId);
    }

    public async Task<QuizQuestionEntity?> GetQuizQuestionsByIdAsync(int quizId)
    {
        return await GetByFilterAsync(e => e.Id == quizId);
    }

    public async Task<bool> PatchQuizQuestionAsync(int quizId, string newQuestion)
    {
        return await PatchAsync(quizId, e => e.Question = newQuestion);
    }

    public async Task<bool> PatchDeleteStatusAsync(int quizId)
    {
        return await PatchAsync(quizId, e => e.IsDeleted = !e.IsDeleted);
    }

    public async Task<bool> DeleteQuizQuestionAsync(int quizId)
    {
        return await DeleteAsync(e => e.Id == quizId);
    }
}