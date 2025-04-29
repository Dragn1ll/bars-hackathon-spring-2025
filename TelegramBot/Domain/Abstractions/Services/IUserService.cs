using Domain.Entities;
using Domain.Models.Dto.Bot;
using Domain.Utils;

namespace Domain.Abstractions.Services;

public interface IUserService
{
    Task<Result<UserEntity>> RegisterAsync(RegisterUserDto registerUserDto);
    Task<Result> LoginAsync(long userId);
    Task<Result> UnregisterAsync(long userId);
}