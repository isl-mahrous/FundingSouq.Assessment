using FundingSouq.Assessment.Api.Infrastructure;
using FundingSouq.Assessment.Application.Commands;
using FundingSouq.Assessment.Core.Dtos;
using FundingSouq.Assessment.Core.Dtos.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FundingSouq.Assessment.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IdentityController : FundingSouqControllerBase
{
    private readonly ISender _sender;

    public IdentityController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Registers a new user for the Hub.
    /// </summary>
    /// <remarks>
    /// This endpoint allows for the registration of a new Hub user by providing necessary details such as email, password, first name, last name, and role.
    /// </remarks>
    /// <param name="request">The registration details of the new Hub user.</param>
    /// <response code="200">Successfully registered the user. Returns the user's login details, including a JWT token.</response>
    /// <response code="400">Registration failed due to validation errors or if the email is already in use.</response>
    [HttpPost("hub-users/register")]
    [ProducesResponseType(typeof(HubUserLoginDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ValidationError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterHubUser([FromBody] RegisterHubUserCommand request)
    {
        Result<HubUserLoginDto> response = await _sender.Send(request);
        return response.IsSuccess ? Ok(response.Value) : BadRequest(response.Error);
    }
    
    /// <summary>
    /// Authenticates a Hub user and returns a JWT token upon successful login.
    /// </summary>
    /// <remarks>
    /// This endpoint verifies the user's credentials (email and password) and issues a JWT token if the login is successful.
    /// </remarks>
    /// <param name="request">The login details of the Hub user, including email and password.</param>
    /// <response code="200">Successfully authenticated the user. Returns the user's login details, including a JWT token.</response>
    /// <response code="400">Login failed due to validation errors or incorrect credentials.</response>
    /// <response code="500">An unexpected error occurred on the server.</response>
    [HttpPost("hub-users/login")]
    [ProducesResponseType(typeof(HubUserLoginDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(ValidationError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> LoginHubUser([FromBody] LoginHubUserCommand request)
    {
        Result<HubUserLoginDto> response = await _sender.Send(request);
        return response.IsSuccess ? Ok(response.Value) : BadRequest(response.Error);
    }
}