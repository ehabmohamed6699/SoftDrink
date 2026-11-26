namespace Billing.Application.Result;

public enum ResultErrorType
{
    None,
    NotFound,
    Validation,
    Conflict,
    Unauthorized,
    Failure
}

public record ResultError(ResultErrorType Type, string Message);