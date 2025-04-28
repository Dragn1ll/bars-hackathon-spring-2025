using Domain.Entities;

namespace Domain.Abstractions.Services;

public interface IJwtWorker
{
    string GenerateToken(AdminEntity user);
}