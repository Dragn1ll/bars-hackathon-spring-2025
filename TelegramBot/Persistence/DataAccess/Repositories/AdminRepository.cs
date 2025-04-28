using Domain.Abstractions.Repositories;
using Domain.Entities;

namespace Persistence.DataAccess.Repositories;

public class AdminRepository(AppDbContext context) : AbstractRepository<AdminEntity>(context), IAdminRepository
{
}