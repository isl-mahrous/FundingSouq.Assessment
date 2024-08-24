namespace FundingSouq.Assessment.Core.Dtos.Common;

/// <summary>
/// Represents the outcome of an operation, indicating success or failure.
/// </summary>
/// <remarks>
/// This class encapsulates the result of an operation, providing information about whether the operation 
/// was successful and, if not, the associated error. It is designed to prevent inconsistent states by enforcing 
/// that a successful result cannot have an error, and an unsuccessful result must have an error.
/// </remarks>
public class Result
{
    /// <summary>
    /// Gets a value indicating whether the operation was successful.
    /// </summary>
    public bool IsSuccess { get; private set; }

    /// <summary>
    /// Gets the error associated with the operation, if any.
    /// </summary>
    public Error Error { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Result"/> class.
    /// </summary>
    /// <param name="isSuccess">Indicates whether the operation was successful.</param>
    /// <param name="error">The error associated with the operation, if any.</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown if a successful result contains an error or an unsuccessful result does not contain an error.
    /// </exception>
    protected Result(bool isSuccess, Error error)
    {
        switch (isSuccess)
        {
            case true when error != null:
                throw new InvalidOperationException("Cannot be successful and contain an error.");
            case false when error == null:
                throw new InvalidOperationException("Cannot be unsuccessful and not contain an error.");
            default:
                IsSuccess = isSuccess;
                Error = error;
                break;
        }
    }

    /// <summary>
    /// Creates a successful <see cref="Result"/> with no error.
    /// </summary>
    /// <returns>A successful <see cref="Result"/>.</returns>
    public static Result Success()
    {
        return new Result(true, null);
    }

    /// <summary>
    /// Creates an unsuccessful <see cref="Result"/> with the specified error.
    /// </summary>
    /// <param name="error">The error associated with the failure.</param>
    /// <returns>An unsuccessful <see cref="Result"/>.</returns>
    public static Result Failure(Error error)
    {
        return new Result(false, error);
    }

    /// <summary>
    /// Implicitly converts an <see cref="Error"/> to a failed <see cref="Result"/>.
    /// </summary>
    /// <param name="error">The error to convert.</param>
    public static implicit operator Result(Error error)
    {
        return Failure(error);
    }
}


/// <summary>
/// Represents the outcome of an operation that returns a value, indicating success or failure.
/// </summary>
/// <typeparam name="T">The type of the value returned by the operation.</typeparam>
/// <remarks>
/// This class extends <see cref="Result"/> to include a value that is returned by a successful operation.
/// It ensures that a successful result contains a value, and an unsuccessful result contains an error.
/// </remarks>
public class Result<T> : Result
{
    /// <summary>
    /// Gets the value returned by the operation, if successful.
    /// </summary>
    public T Value { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Result{T}"/> class.
    /// </summary>
    /// <param name="isSuccess">Indicates whether the operation was successful.</param>
    /// <param name="value">The value returned by the operation, if successful.</param>
    /// <param name="error">The error associated with the operation, if any.</param>
    private Result(bool isSuccess, T value, Error error)
        : base(isSuccess, error)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a successful <see cref="Result{T}"/> with the specified value.
    /// </summary>
    /// <param name="value">The value returned by the operation.</param>
    /// <returns>A successful <see cref="Result{T}"/>.</returns>
    public static Result<T> Success(T value)
    {
        return new Result<T>(true, value, null);
    }

    /// <summary>
    /// Creates an unsuccessful <see cref="Result{T}"/> with the specified error.
    /// </summary>
    /// <param name="error">The error associated with the failure.</param>
    /// <returns>An unsuccessful <see cref="Result{T}"/>.</returns>
    public new static Result<T> Failure(Error error)
    {
        return new Result<T>(false, default, error);
    }

    /// <summary>
    /// Implicitly converts a value of type <typeparamref name="T"/> to a successful <see cref="Result{T}"/>.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    public static implicit operator Result<T>(T value)
    {
        return Success(value);
    }
    
    /// <summary>
    /// Implicitly converts an <see cref="Error"/> to a failed <see cref="Result{T}"/>.
    /// </summary>
    /// <param name="error">The error to convert.</param>
    public static implicit operator Result<T>(Error error)
    {
        return Failure(error);
    }
}
