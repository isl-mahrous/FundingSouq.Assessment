using FundingSouq.Assessment.Core.Dtos.Common;
using Microsoft.AspNetCore.Diagnostics;

namespace FundingSouq.Assessment.Api.Infrastructure;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;
    private readonly IWebHostEnvironment _environment;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IWebHostEnvironment environment)
    {
        _logger = logger;
        _environment = environment;
    }
    
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext, 
        Exception exception,
        CancellationToken cancellationToken)
    {
        
        // Log the exception in a structured way
        _logger.LogError(exception, "Exception occurred. {Message}", exception.Message);
        
        var message  = _environment.IsDevelopment() 
            ? exception.Message 
            : "An error occurred while processing your request. Please try again later.";
        
        await httpContext.Response.WriteAsJsonAsync(new Error("INTERNAL_SERVER_ERROR", message), cancellationToken);
        
        return true;
    }
}