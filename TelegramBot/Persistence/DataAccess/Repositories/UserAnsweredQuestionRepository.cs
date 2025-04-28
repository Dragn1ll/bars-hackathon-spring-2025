using Domain.Abstractions.Repositories;
using Domain.Entities;

namespace Persistence.DataAccess.Repositories;

public class UserAnsweredQuestionRepository(AppDbContext context) 
    : AbstractRepository<UserAnsweredQuestionEntity>(context), IUserAnsweredQuestionRepository
{
    
}