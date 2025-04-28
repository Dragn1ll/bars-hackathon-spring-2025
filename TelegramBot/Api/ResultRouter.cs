using Domain.Models.Enums;
using Domain.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Api;

public static class ResultRouter
{
    public static ObjectResult GetActionResult(Result result)
    {
        if (result.IsSuccess)
        {
            return new OkObjectResult(result);
        }

        if (result.Error is null)
        {
            return new ObjectResult("Something get wrong"){StatusCode = 500};
        }

        switch (result.Error.ErrorType)
        {
            case ErrorType.BadRequest:
                return new BadRequestObjectResult(result.Error);
            case ErrorType.ServerError:
                return new ObjectResult(result.Error.Message){StatusCode = 500};
            case ErrorType.NotFound:
                return new NotFoundObjectResult(result.Error);
            default:
                return new ObjectResult("");
        }
    }
}