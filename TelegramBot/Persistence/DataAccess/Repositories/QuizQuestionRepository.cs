using Domain.Abstractions.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Persistence.DataAccess.Repositories;

public class QuizQuestionRepository(AppDbContext context) : 
    AbstractRepository<QuizQuestionEntity>(context), 
    IQuizQuestionRepository
{
    public async Task<QuizQuestionEntity?> GetQuestionWithOptions(Guid questionId)
    {
        return await context.Set<QuizQuestionEntity>()
            .AsNoTracking()
            .Include(e => e.QuizOptions)
            .FirstOrDefaultAsync(question => question.QuestionId == questionId);
    }
}