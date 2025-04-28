using Domain.Abstractions.Repositories;
using Domain.Abstractions.Services;
using Domain.Entities;
using Domain.Models.Dto.Admin;
using Domain.Models.Enums;
using Domain.Utils;

namespace Application.Services;

public class AdminService(IUnitOfWork unitOfWork, Mapper mapper, IJwtWorker jwtWorker) 
    : IAdminService
{
    public async Task<Result> Register(AdminLoginRegisterRequestDto adminLoginRegisterRequestDto)
    {
        try
        {
            if (string.IsNullOrEmpty(adminLoginRegisterRequestDto.Username) 
                || string.IsNullOrEmpty(adminLoginRegisterRequestDto.PasswordHash))
                return Result.Failure(
                    new Error(ErrorType.BadRequest, "Invalid username or password"));
            
            if (await ThereIsAAdmin(adminLoginRegisterRequestDto.Username) != null)
                return Result.Failure(
                    new Error(ErrorType.BadRequest, "Existing user with that username"));

            var entity = mapper.Map<AdminLoginRegisterRequestDto, AdminEntity>(adminLoginRegisterRequestDto);
            entity.AdminId = Guid.NewGuid();

            var result = await unitOfWork.Admins.AddAsync(entity);
            await unitOfWork.SaveChangesAsync();
            
            return result 
                ? Result.Success()
                : Result.Failure(new Error(ErrorType.ServerError, "Can't log in"));
        }
        catch (Exception exception)
        {
            return Result<bool>.Failure(new Error(ErrorType.ServerError, exception.Message));
        }
    }

    public async Task<Result<string>> Login(AdminLoginRegisterRequestDto adminLoginRegisterRequestDto)
    {
        try
        {
            if (string.IsNullOrEmpty(adminLoginRegisterRequestDto.Username) 
                || string.IsNullOrEmpty(adminLoginRegisterRequestDto.PasswordHash))
                return Result<string>.Failure(
                    new Error(ErrorType.BadRequest, "Invalid username or password"));

            var admin = await ThereIsAAdmin(adminLoginRegisterRequestDto.Username);
            
            if (admin == null)
                return Result<string>.Failure(
                    new Error(ErrorType.NotFound, "User does not exist"));

            if (admin.PasswordHash != adminLoginRegisterRequestDto.PasswordHash)
                return Result<string>.Failure(
                    new Error(ErrorType.BadRequest, "Invalid password"));
            
            return Result<string>.Success(jwtWorker.GenerateToken(admin));
        }
        catch (Exception exception)
        {
            return Result<string>.Failure(new Error(ErrorType.ServerError, exception.Message));
        }
    }

    private async Task<AdminEntity?> ThereIsAAdmin(string username)
    {
        return await unitOfWork.Admins
            .GetByFilterAsync(a => a.Username == username);
    }
}