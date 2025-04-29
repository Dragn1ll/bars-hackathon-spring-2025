using Domain.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Filters;

public class TelegramUserAuthFilter(IUserService userService): IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        Console.WriteLine(context.HttpContext.Request.Headers["x-user-id"]);
        var userIdHeader = context.HttpContext.Request.Headers["x-user-id"].FirstOrDefault();
        
        if (string.IsNullOrEmpty(userIdHeader))
        {
            context.Result = new BadRequestResult();
            return;
        }

        if (!long.TryParse(userIdHeader, out var userId)) 
        {
            context.Result = new BadRequestResult();
            return;
        }
        
        var userLoginResult = await userService.LoginAsync(userId);

        if (!userLoginResult.IsSuccess)
        {
            context.Result = ResultRouter.GetActionResult(userLoginResult);
        }
    }
}