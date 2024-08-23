using FluentValidation;
using FundingSouq.Assessment.Core.Dtos.Common;
using MediatR;

namespace FundingSouq.Assessment.Application.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!_validators.Any())
            return await next();

        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            _validators.Select(v =>
                v.ValidateAsync(context, cancellationToken)));

        var failures = validationResults
            .Where(r => r.Errors.Count != 0)
            .SelectMany(r => r.Errors)
            .ToList();

        if (failures.Count == 0) return await next();

        var dictionary = failures
            .GroupBy(s => s.PropertyName)
            .ToDictionary(g => g.Key, g => g.Select(s => s.ErrorMessage).ToArray());
        
        var validationError = new ValidationError(dictionary);

        // Use static Failure method to create the appropriate result
        if (typeof(TResponse).IsGenericType && typeof(TResponse).GetGenericTypeDefinition() == typeof(Result<>))
        {
            // Dynamically create Result<T> using the Failure static method
            var genericArgument = typeof(TResponse).GetGenericArguments()[0];
            var resultType = typeof(Result<>).MakeGenericType(genericArgument);

            // Locate and invoke the static Failure method
            var failureMethod = resultType.GetMethod(nameof(Result<TResponse>.Failure), new[] { typeof(Error) });
            var failureResult = failureMethod!.Invoke(null, new object[] { validationError });

            return (TResponse)failureResult;
        }

        // If TResponse is non-generic Result, return as is
        return (TResponse)(object)Result.Failure(validationError);
    }
}