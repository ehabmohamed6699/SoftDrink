namespace Billing.Application.Result;

public class Result
{
    public bool IsSuccess { get; }
    public ResultError? Error { get; }
    protected Result(bool success, ResultError? error) { IsSuccess = success; Error = error; }
    public static Result Success() => new(true, null);
    public static Result Failure(ResultError error) => new(false, error);
    public static Result NotFound(string msg) => Failure(new(ResultErrorType.NotFound, msg));
    public static Result Validation(string msg) => Failure(new(ResultErrorType.Validation, msg));
    public static Result Conflict(string msg) => Failure(new(ResultErrorType.Conflict, msg));
    public static Result Unauthorized(string msg) => Failure(new(ResultErrorType.Unauthorized, msg));
}

public class Result<T> : Result
{
    public T? Value { get; }
    private Result(bool success, T? value, ResultError? error) : base(success, error) { Value = value; }
    public static Result<T> Success(T value) => new(true, value, null);
    public static new Result<T> Failure(ResultError error) => new(false, default, error);
    public static new Result<T> NotFound(string msg) => Failure(new(ResultErrorType.NotFound, msg));
    public static new Result<T> Validation(string msg) => Failure(new(ResultErrorType.Validation, msg));
    public static new Result<T> Conflict(string msg) => Failure(new(ResultErrorType.Conflict, msg));
    public static new Result<T> Unauthorized(string msg) => Failure(new(ResultErrorType.Unauthorized, msg));
}