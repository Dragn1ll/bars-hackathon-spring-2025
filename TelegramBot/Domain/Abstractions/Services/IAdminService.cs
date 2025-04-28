using Domain.Models.Dto.Admin;
using Domain.Utils;

namespace Domain.Abstractions.Services;

public interface IAdminService
{
    Task<Result<string>> Login(AdminLoginRegisterRequestDto adminLoginRegisterRequestDto);
    Task<Result> Register(AdminLoginRegisterRequestDto adminLoginRegisterRequestDto);
}