namespace Billing.Application.Result;

public interface IResultBuilder
{
    Result Success();
    Result<T> Success<T>(T value);
    Result Failure(ResultError error);
    Result<T> Failure<T>(ResultError error);
    Result NotFound(string msg);
    Result<T> NotFound<T>(string msg);
    Result Validation(string msg);
    Result<T> Validation<T>(string msg);
    Result Conflict(string msg);
    Result<T> Conflict<T>(string msg);
    Result Unauthorized(string msg);
    Result<T> Unauthorized<T>(string msg);
}

public class ResultBuilder : IResultBuilder
{
    public Result Success() => Result.Success();
    public Result<T> Success<T>(T value) => Result<T>.Success(value);
    public Result Failure(ResultError error) => Result.Failure(error);
    public Result<T> Failure<T>(ResultError error) => Result<T>.Failure(error);
    public Result NotFound(string msg) => Result.NotFound(msg);
    public Result<T> NotFound<T>(string msg) => Result<T>.NotFound(msg);
    public Result Validation(string msg) => Result.Validation(msg);
    public Result<T> Validation<T>(string msg) => Result<T>.Validation(msg);
    public Result Conflict(string msg) => Result.Conflict(msg);
    public Result<T> Conflict<T>(string msg) => Result<T>.Conflict(msg);
    public Result Unauthorized(string msg) => Result.Unauthorized(msg);
    public Result<T> Unauthorized<T>(string msg) => Result<T>.Unauthorized(msg);
}