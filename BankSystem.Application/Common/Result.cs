namespace BankSystem.Application.Common;

public class Result
{
    public bool IsSuccess { get; set; }
    public Error? Error { get; set; }
    
    public Result(bool isSuccess, Error error)
    {
        IsSuccess = isSuccess;
        Error = error;
    }
    
    public static Result Success() => new Result(true, null!);
    public static Result Fail(Error error) => new Result(false, error);
}

public class Result<T> : Result
{
    public T Value { get; set; }
    
    public Result(bool isSuccess, Error error, T value) : base(isSuccess, error)
    {
        Value = value;
    }
    
    public static Result<T> Success(T value) => new Result<T>(true, null!, value);
    public new static Result<T> Fail(Error error) => new Result<T>(false, error, default(T)!);
}