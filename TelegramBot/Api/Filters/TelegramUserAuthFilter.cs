using Domain.Abstractions.Services;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Filters;

public class TelegramUserAuthFilter(IUserService userService): IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var userId = long.Parse(context.HttpContext.Request.Headers["X-User-Id"].FirstOrDefault() ?? "-1");
        var userLoginResult = await userService.LoginAsync(userId);

        if (!userLoginResult.IsSuccess)
        {
            context.Result = ResultRouter.GetActionResult(userLoginResult);
        }
    }
}