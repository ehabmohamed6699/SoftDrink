using Billing.Application.Result;
using Microsoft.AspNetCore.Mvc;

namespace Billing.Api.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToActionResult(this Result result)
    {
        if (result.IsSuccess) return new OkResult();
        return result.Error?.Type switch
        {
            ResultErrorType.NotFound => new NotFoundObjectResult(result.Error.Message),
            ResultErrorType.Validation => new BadRequestObjectResult(result.Error.Message),
            ResultErrorType.Conflict => new ConflictObjectResult(result.Error.Message),
            ResultErrorType.Unauthorized => new UnauthorizedObjectResult(result.Error.Message),
            _ => new ObjectResult(result.Error?.Message) { StatusCode = 500 }
        };
    }

    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        if (result.IsSuccess) return new OkObjectResult(result.Value);
        return ((Result)result).ToActionResult();
    }
}