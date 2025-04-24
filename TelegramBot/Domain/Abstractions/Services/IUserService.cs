using Domain.Entities;

namespace Domain.Abstractions.Services;

public interface IUserService
{
    Task<UserEntity> RegisterAsync(UserEntity user);
    Task<bool> LoginAsync(long chatId);
    Task<bool> UnregisterAsync(long chatId);
}