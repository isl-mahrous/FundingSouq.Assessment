using FundingSouq.Assessment.Application.Commands;
using FundingSouq.Assessment.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FundingSouq.Assessment.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IdentityController : ControllerBase
{
    private readonly ISender _sender;

    public IdentityController(ISender sender)
    {
        _sender = sender;
    }
    
    [HttpPost("register-hub-user")]
    public async Task<IActionResult> RegisterHubUser([FromBody] RegisterHubUserCommand request)
    {
        var response = await _sender.Send(request);
        return response.IsSuccess ? Ok(response.Value) : BadRequest(response.Error);
    }
    
    [HttpPost("test-auth")]
    [Authorize(Policy = nameof(UserType.Client))]
    public IActionResult TestAuth()
    {
        return Ok("Authorized");
    }
}