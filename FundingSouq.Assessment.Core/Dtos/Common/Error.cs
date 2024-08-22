namespace FundingSouq.Assessment.Core.Dtos.Common;

public class Error
{
    public string Code { get; private set; }
    public string Message { get; private set; }

    public Error(string code, string message)
    {
        Code = code;
        Message = message;
    }
}

public class ValidationError : Error
{
    public IDictionary<string, string[]> ValidationErrors { get; set; }

    public ValidationError(IDictionary<string, string[]> validationErrors)
        : base("VALIDATION_ERROR", "There are validation errors. Please see validation errors for more details.")
    {
        ValidationErrors = validationErrors;
    }
}