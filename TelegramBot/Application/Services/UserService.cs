using Domain.Abstractions.Repositories;
using Domain.Abstractions.Services;
using Domain.Entities;
using Domain.Models.Dto;
using Domain.Utils;

namespace Application.Services;

public class UserService(IUnitOfWork unitOfWork): IUserService
{
    public async Task<Result<UserEntity>> RegisterAsync(RegisterUserDto registerUserDto)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> LoginAsync(long userId)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> UnregisterAsync(long userId)
    {
        throw new NotImplementedException();
    }
}