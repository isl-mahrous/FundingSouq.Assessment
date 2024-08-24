namespace FundingSouq.Assessment.Core.Dtos.Common;

/// <summary>
/// Represents a standardized error with a code and message.
/// </summary>
/// <remarks>
/// This class is used to encapsulate error information, providing a code that identifies 
/// the type of error and a message that describes the error in detail.
/// </remarks>
public class Error
{
    /// <summary>
    /// Gets the error code that identifies the type of error.
    /// </summary>
    public string Code { get; private set; }

    /// <summary>
    /// Gets the error message that describes the error.
    /// </summary>
    public string Message { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Error"/> class with a specified code and message.
    /// </summary>
    /// <param name="code">The error code that identifies the type of error.</param>
    /// <param name="message">The error message that describes the error.</param>
    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }
}