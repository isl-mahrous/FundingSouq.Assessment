using FundingSouq.Assessment.Api.Infrastructure;
using FundingSouq.Assessment.Application.Commands;
using FundingSouq.Assessment.Application.Queries;
using FundingSouq.Assessment.Core.Dtos;
using FundingSouq.Assessment.Core.Dtos.Common;
using FundingSouq.Assessment.Core.Enums;
using MediatR;

namespace FundingSouq.Assessment.Api.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

    /// <summary>
    /// Retrieves account details by account ID.
    /// </summary>
    /// <param name="accountId">The ID of the account to retrieve.</param>
    /// <returns>Account details if found; otherwise, an error response.</returns>
    /// <remarks>
    /// This API endpoint retrieves details of an account based on the provided account ID.
    /// It is accessible to authorized users and returns the account details if the account exists.
    /// </remarks>
    [HttpGet("{accountId:int}")]
    [ProducesResponseType(typeof(AccountDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAccountById(int accountId)
    {
        var response = await _sender.Send(new AccountByIdQuery { Id = accountId });
        return response.IsSuccess ? Ok(response.Value) : BadRequest(response.Error);
    }

    /// <summary>
    /// Creates a new account for a client.
    /// </summary>
    /// <param name="request">The request containing account details.</param>
    /// <returns>The created account details.</returns>
    /// <remarks>
    /// This API endpoint allows an admin to create a new account for a client.
    /// The request must include the account number, and account type.
    /// </remarks>
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

    /// <summary>
    /// Updates an existing client account.
    /// </summary>
    /// <param name="request">The request containing updated account details.</param>
    /// <returns>The updated account details.</returns>
    /// <remarks>
    /// This API endpoint allows an admin to update an existing client account.
    /// The request must include the account ID, new account number, and account type.
    /// </remarks>
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

    /// <summary>
    /// Deletes an existing client account by ID.
    /// </summary>
    /// <param name="accountId">The ID of the account to delete.</param>
    /// <returns>A success message if the account is deleted; otherwise, an error response.</returns>
    /// <remarks>
    /// This API endpoint allows an admin to delete a client account based on the provided account ID.
    /// If the account is the only one associated with the client, it cannot be deleted.
    /// </remarks>
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
