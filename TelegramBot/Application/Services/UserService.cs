using Domain.Abstractions.Repositories;
using Domain.Abstractions.Services;
using Domain.Entities;
using Domain.Models.Dto.Bot;
using Domain.Models.Enums;
using Domain.Utils;

namespace Application.Services;

public class UserService(IUnitOfWork unitOfWork, Mapper mapper): IUserService
{
    public async Task<Result<Admin>> RegisterAsync(RegisterUserDto registerUserDto)
    {
        try
        {
            if (await unitOfWork.Users.GetByFilterAsync(u => u.UserId 
                                                              == registerUserDto.UserId)
                 != null)
                return Result<Admin>.Failure(
                    new Error(ErrorType.BadRequest, "User already exists"));

            var user = mapper.Map<RegisterUserDto, Admin>(registerUserDto);
            var result = await unitOfWork.Users.AddAsync(user);
            await unitOfWork.SaveChangesAsync();
            
            return result 
                ? Result<Admin>.Success(user)
                : Result<Admin>.Failure(
                    new Error(ErrorType.ServerError, "Can't register user"));
        }
        catch (Exception exception)
        {
            return Result<Admin>.Failure(new Error(ErrorType.ServerError, exception.Message));
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
            return Result<Admin>.Failure(new Error(ErrorType.ServerError, exception.Message));
        }
    }

    public async Task<Result> UnregisterAsync(long userId)
    {
        try
        {
            if (await unitOfWork.Users.GetByFilterAsync(u => u.UserId == userId) == null)
                return Result.Failure(new Error(ErrorType.NotFound, "User does not exist"));

            var result = await unitOfWork.Users.DeleteAsync(u => u.UserId == userId);
            await unitOfWork.SaveChangesAsync();
            
            return result
                ? Result.Success()
                : Result.Failure(new Error(ErrorType.ServerError, "Can't unregister user"));
        }
        catch (Exception exception)
        {
            return Result<Admin>.Failure(new Error(ErrorType.ServerError, exception.Message));
        }
    }
}