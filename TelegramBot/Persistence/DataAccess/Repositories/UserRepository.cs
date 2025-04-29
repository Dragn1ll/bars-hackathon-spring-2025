using Domain.Abstractions.Repositories;
using Domain.Entities;

namespace Persistence.DataAccess.Repositories;

public class UserRepository(AppDbContext context) : 
    AbstractRepository<UserEntity>(context), 
    IUserRepository
{
}