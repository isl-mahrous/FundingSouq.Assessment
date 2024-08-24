namespace FundingSouq.Assessment.Core.Dtos.Common;

/// <summary>
/// Represents a validation error that extends the standard <see cref="Error"/> class.
/// </summary>
/// <remarks>
/// This class is used to encapsulate validation errors, providing a collection of 
/// validation error messages for specific fields, in addition to the standard error code and message.
/// </remarks>
public class ValidationError : Error
{
    /// <summary>
    /// Gets or sets the dictionary of validation errors, where the key is the field name
    /// and the value is an array of error messages related to that field.
    /// </summary>
    public IDictionary<string, string[]> ValidationErrors { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationError"/> class with specified validation errors.
    /// </summary>
    /// <param name="validationErrors">A dictionary containing validation errors with field names as keys 
    /// and an array of error messages as values.</param>
    public ValidationError(IDictionary<string, string[]> validationErrors)
        : base("VALIDATION_ERROR", "There are validation errors. Please see validation errors for more details.")
    {
        ValidationErrors = validationErrors;
    }
}
