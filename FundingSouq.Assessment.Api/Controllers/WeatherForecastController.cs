using System.Text;
using System.Text.Json;
using FluentValidation;
using FundingSouq.Assessment.Core.Dtos.Common;
using FundingSouq.Assessment.Core.Entities;
using FundingSouq.Assessment.Infrastructure.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace FundingSouq.Assessment.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }
 
    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get([FromServices] IDistributedCache cache)
    {
        if(cache.Get("WeatherForecast") != null)
        {
            return JsonSerializer.Deserialize<IEnumerable<WeatherForecast>>(cache.Get("WeatherForecast")) ?? Array.Empty<WeatherForecast>();
        }
        var result =  Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
            
        cache.Set("WeatherForecast", Encoding.UTF8.GetBytes(JsonSerializer.Serialize(result)), new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10)
        });
        
        return result;
    }
    
    [HttpGet("countries")]
    public IEnumerable<Country> GetCountries([FromServices] IDistributedCache cache,  [FromServices] AppDbContext context)
    {
        if(cache.Get("Countries") != null)
        {
            return JsonSerializer.Deserialize<IEnumerable<Country>>(cache.Get("Countries")) ?? Array.Empty<Country>();
        }
        var result = context.Countries.ToList();
        
        cache.Set("Countries", Encoding.UTF8.GetBytes(JsonSerializer.Serialize(result)), new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10)
        });
        
        return result;
    }
    
    [HttpPost("test-registeration")]
    public IActionResult TestRegisteration([FromBody] Client user, [FromServices] IServiceProvider serviceProvider)
    {
        // get validator for client
        var validator = serviceProvider.GetRequiredService<IValidator<Client>>();
        var res = validator.Validate(user);
        if (!res.IsValid)
        {
            var error = new ValidationError(res.ToDictionary());
            var result = Result.Failure(error);
            return BadRequest(result);
        }
        return Ok(Result.Success());
    }
}
