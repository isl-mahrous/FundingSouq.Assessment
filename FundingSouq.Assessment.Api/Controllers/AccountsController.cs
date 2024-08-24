using FundingSouq.Assessment.Api.Infrastructure;
using FundingSouq.Assessment.Application.Commands;
using FundingSouq.Assessment.Application.Queries;
using FundingSouq.Assessment.Core.Dtos;
using FundingSouq.Assessment.Core.Dtos.Common;
using FundingSouq.Assessment.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FundingSouq.Assessment.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = nameof(UserType.HubUser))]
public class AccountsController : FundingSouqControllerBase
{
    private readonly ISender _sender;

    public AccountsController(ISender sender)
    {
        _sender = sender;
    }
    
    [HttpGet("{accountId:int}")]
    [ProducesResponseType(typeof(AccountDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAccountById(int accountId)
    {
        var response = await _sender.Send(new AccountByIdQuery { Id = accountId });
        return response.IsSuccess ? Ok(response.Value) : BadRequest(response.Error);
    }
    
    [HttpPost("create")]
    [Authorize(Roles = nameof(HubUserRole.Admin))]
    [ProducesResponseType(typeof(AccountDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateAccount([FromBody] CreateClientAccountCommand request)
    {
        var response = await _sender.Send(request);
        return response.IsSuccess ? Ok(response.Value) : BadRequest(response.Error);
    }

    [HttpPut("update")]
    [Authorize(Roles = nameof(HubUserRole.Admin))]
    [ProducesResponseType(typeof(AccountDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateAccount([FromBody] UpdateClientAccountCommand request)
    {
        var response = await _sender.Send(request);
        return response.IsSuccess ? Ok(response.Value) : BadRequest(response.Error);
    }

    [HttpDelete("delete/{accountId:int}")]
    [Authorize(Roles = nameof(HubUserRole.Admin))]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteAccount(int accountId)
    {
        var response = await _sender.Send(new DeleteClientAccountCommand { Id = accountId });
        return response.IsSuccess ? Ok("Account deleted successfully") : BadRequest(response.Error);
    }
}