using FundingSouq.Assessment.Core.Dtos.Common;
using Microsoft.AspNetCore.Diagnostics;

namespace FundingSouq.Assessment.Api.Infrastructure;

/// <summary>
/// Handles global exceptions by logging them and returning a standardized error response.
/// </summary>
public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IWebHostEnvironment _environment;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IWebHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;
    }
    
    /// <inheritdoc />
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, 
        Exception exception,
        CancellationToken cancellationToken)
    {
        // Log the exception with details
        _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);
        
        // Return detailed error in development, generic error in production
        var message = _environment.IsDevelopment() 
            ? exception.Message 
            : "An error occurred. Please try again later.";
        
        await httpContext.Response.WriteAsJsonAsync(new Error("INTERNAL_SERVER_ERROR", message), cancellationToken);
        
        return true; // Indicate that the exception was handled
    }
}