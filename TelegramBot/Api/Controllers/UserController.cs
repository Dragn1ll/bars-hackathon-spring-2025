using Domain.Abstractions.Services;
using Domain.Models.Dto.Bot;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("users")]
public class UserController(IUserService userService): ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserDto registerUserDto)
    {
        var result = await userService.RegisterAsync(registerUserDto);
        return ResultRouter.GetActionResult(result);
    }
    
    
}