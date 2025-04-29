using Domain.Abstractions.Services;
using Domain.Models.Dto.Admin;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("admin")]
public class AdminController(IAdminService adminService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(AdminLoginRegisterRequestDto adminRegisterRequestDto)
    {
        var result = await adminService.Register(adminRegisterRequestDto);
        return ResultRouter.GetActionResult(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(AdminLoginRegisterRequestDto adminLoginRequestDto)
    {
        var result = await adminService.Login(adminLoginRequestDto);
        
        if (result.IsSuccess)
            HttpContext.Response.Cookies.Append("jwt-secret", result.Value!);
        
        return ResultRouter.GetActionResult(result);
    }
}