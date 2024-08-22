namespace FundingSouq.Assessment.Core.Dtos.Common;

public class Result
{
    public bool IsSuccess { get; private set; }
    public Error Error { get; private set; }
    public bool IsFailure => !IsSuccess;

    protected Result(bool isSuccess, Error error)
    {
        if (isSuccess && error != null)
            throw new InvalidOperationException();
        if (!isSuccess && error == null)
            throw new InvalidOperationException();

        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success()
    {
        return new Result(true, null);
    }

    public static Result Failure(Error error)
    {
        return new Result(false, error);
    }
    
    public static implicit operator Result(Error error)
    {
        return Failure(error);
    }
}

public class Result<T> : Result
{
    public T Value { get; private set; }

    private Result(bool isSuccess, T value, Error error)
        : base(isSuccess, error)
    {
        Value = value;
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(true, value, null);
    }

    public new static Result<T> Failure(Error error)
    {
        return new Result<T>(false, default(T), error);
    }

    public static implicit operator Result<T>(T value)
    {
        return Success(value);
    }
    
    public static implicit operator Result<T>(Error error)
    {
        return Failure(error);
    }
}
