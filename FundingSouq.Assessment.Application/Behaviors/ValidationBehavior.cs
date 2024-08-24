using FluentValidation;
using FundingSouq.Assessment.Core.Dtos.Common;
using MediatR;

namespace FundingSouq.Assessment.Application.Behaviors;

/// <summary>
/// Pipeline behavior for validating requests using FluentValidation.
/// </summary>
/// <typeparam name="TRequest">The type of the request.</typeparam>
/// <typeparam name="TResponse">The type of the response.</typeparam>
public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    /// <inheritdoc />
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next(); // Proceed if no validators are found

        var context = new ValidationContext<TRequest>(request);

        // Run all validators asynchronously
        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        // Collect all validation failures
        var failures = validationResults
            .Where(r => r.Errors.Count > 0)
            .SelectMany(r => r.Errors)
            .ToList();

        // If no failures, continue to the next behavior in the pipeline
        if (failures.Count == 0) return await next();

        // Group failures by property name and create a ValidationError object
        var dictionary = failures
            .GroupBy(s => s.PropertyName)
            .ToDictionary(g => g.Key, g => g.Select(s => s.ErrorMessage).ToArray());
        
        var validationError = new ValidationError(dictionary);

        // Handle generic Result<T> response types
        if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
        {
            var genericArgument = typeof(TResponse).GetGenericArguments()[0];
            var resultType = typeof(Result<>).MakeGenericType(genericArgument);

            // Invoke the static Failure method on Result<T>
            var failureMethod = resultType.GetMethod(nameof(Result<TResponse>.Failure), new[] { typeof(Error) });
            var failureResult = failureMethod!.Invoke(null, new object[] { validationError });

            return (TResponse)failureResult;
        }

        // Handle non-generic Result response types
        return (TResponse)(object)Result.Failure(validationError);
    }
}