using Domain.Abstractions.Repositories;
using Domain.Entities;

namespace Persistence.DataAccess.Repositories;

public class QuizQuestionRepository(AppDbContext context) : 
    AbstractRepository<QuizQuestionEntity>(context), 
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

    public async Task<IEnumerable<QuizQuestionEntity?>> GetQuizQuestionsByLessonIdAsync(Guid lessonId)
    {
        return await GetAllByFilterAsync(e => e.LessonId == lessonId);
    }

    public async Task<QuizQuestionEntity?> GetQuizQuestionsByIdAsync(Guid quizId)
    {
        return await GetByFilterAsync(e => e.QuestionId == quizId);
    }

    public async Task<bool> PatchQuizQuestionAsync(Guid quizId, string newQuestion)
    {
        return await PatchAsync(quizId, e => e.QuestionText = newQuestion);
    }

    public async Task<bool> PatchDeleteStatusAsync(Guid quizId)
    {
        return await PatchAsync(quizId, e => e.IsDeleted = !e.IsDeleted);
    }

    public async Task<bool> DeleteQuizQuestionAsync(Guid quizId)
    {
        return await DeleteAsync(e => e.QuestionId == quizId);
    }
}