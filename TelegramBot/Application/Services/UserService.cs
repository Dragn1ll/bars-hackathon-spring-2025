using Domain.Abstractions.Repositories;
using Domain.Abstractions.Services;
using Domain.Entities;
using Domain.Models.Dto.Bot;
using Domain.Models.Enums;
using Domain.Utils;

namespace Application.Services;

public class UserService(IUnitOfWork unitOfWork, Mapper mapper): IUserService
{
    public async Task<Result<UserEntity>> RegisterAsync(RegisterUserDto registerUserDto)
    {
        try
        {
            if (await unitOfWork.Users.GetByFilterAsync(u => u.UserId 
                                                              == registerUserDto.UserId)
                 != null)
                return Result<UserEntity>.Failure(
                    new Error(ErrorType.BadRequest, "User already exists"));

            var user = mapper.Map<RegisterUserDto, UserEntity>(registerUserDto);
            
            return await unitOfWork.Users.AddAsync(user) 
                ? Result<UserEntity>.Success(user)
                : Result<UserEntity>.Failure(
                    new Error(ErrorType.ServerError, "Can't register user"));
        }
        catch (Exception exception)
        {
            return Result<UserEntity>.Failure(new Error(ErrorType.ServerError, exception.Message));
        }
    }

    public async Task<Result> LoginAsync(long userId)
    {
        try
        {
            return await unitOfWork.Users.GetByFilterAsync(u => u.UserId == userId) != null 
                ? Result.Success()
                : Result.Failure(new Error(ErrorType.NotFound, "User does not exist"));
        }
        catch (Exception exception)
        {
            return Result<UserEntity>.Failure(new Error(ErrorType.ServerError, exception.Message));
        }
    }

    public async Task<Result> UnregisterAsync(long userId)
    {
        try
        {
            if (await unitOfWork.Users.GetByFilterAsync(u => u.UserId == userId) == null)
                return Result.Failure(new Error(ErrorType.NotFound, "User does not exist"));
            
            return await unitOfWork.Users.DeleteAsync(u => u.UserId == userId)
                ? Result.Success()
                : Result.Failure(new Error(ErrorType.ServerError, "Can't unregister user"));
        }
        catch (Exception exception)
        {
            return Result<UserEntity>.Failure(new Error(ErrorType.ServerError, exception.Message));
        }
    }
}