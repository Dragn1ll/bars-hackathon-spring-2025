using Domain.Entities;
using Domain.Utils;

namespace Domain.Abstractions.Services;

public interface IUserService
{
    Task<Result<UserEntity>> RegisterAsync(UserEntity user);
    Task<Result> LoginAsync(long chatId);
    Task<Result> UnregisterAsync(long chatId);
}