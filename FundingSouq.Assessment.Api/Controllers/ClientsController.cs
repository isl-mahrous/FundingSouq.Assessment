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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = nameof(UserType.HubUser))]
public class ClientsController : FundingSouqControllerBase
{
    private readonly ISender _sender;

    public ClientsController(ISender sender)
    {
        _sender = sender;
    }

    /// <summary>
    /// Retrieves a paged list of clients.
    /// </summary>
    /// <param name="searchKey">Optional search term to filter clients.</param>
    /// <param name="sortKey">Field by which to sort the clients.</param>
    /// <param name="sortDirection">Direction to sort the clients.</param>
    /// <param name="page">Page number to retrieve.</param>
    /// <param name="pageSize">Number of clients per page.</param>
    /// <returns>Paged list of clients matching the criteria.</returns>
    /// <remarks>
    /// This endpoint retrieves a paged list of clients based on search, sorting, and paging parameters.
    /// It is intended for use by authorized hub users.
    /// </remarks>
    [HttpGet("paged")]
    [ProducesResponseType(typeof(PagedResult<ClientDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetClientsPaged(
        string searchKey = "",
        string sortKey = "id",
        SortDirection sortDirection = SortDirection.Ascending,
        int page = 1,
        int pageSize = 10
    )
    {
        var result = await _sender.Send(new ClientsAsPagedQuery
        {
            UserId = GetUserId(),
            SearchKey = searchKey,
            Page = page,
            PageSize = pageSize,
            SortKey = sortKey,
            SortDirection = sortDirection
        });
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    /// <summary>
    /// Retrieves details of a specific client by ID.
    /// </summary>
    /// <param name="clientId">The ID of the client to retrieve.</param>
    /// <returns>Client details if found; otherwise, an error response.</returns>
    /// <remarks>
    /// This endpoint retrieves details of a specific client based on the provided client ID.
    /// It is accessible to authorized hub users.
    /// </remarks>
    [HttpGet("{clientId}")]
    [ProducesResponseType(typeof(ClientDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetClientById(int clientId)
    {
        var result = await _sender.Send(new ClientByIdQuery { ClientId = clientId });
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    /// <summary>
    /// Creates a new client.
    /// </summary>
    /// <param name="command">The command containing client details.</param>
    /// <returns>The created client details.</returns>
    /// <remarks>
    /// This endpoint allows an admin to create a new client.
    /// The request must include all necessary client information.
    /// </remarks>
    [HttpPost("create")]
    [Authorize(Roles = nameof(HubUserRole.Admin))]
    [ProducesResponseType(typeof(ClientDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateClient([FromForm] CreateClientCommand command)
    {
        var result = await _sender.Send(command);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    /// <summary>
    /// Updates an existing client.
    /// </summary>
    /// <param name="command">The command containing updated client details.</param>
    /// <returns>The updated client details.</returns>
    /// <remarks>
    /// This endpoint allows an admin to update the details of an existing client.
    /// The request must include the client ID and the updated information.
    /// </remarks>
    [HttpPut("update")]
    [Authorize(Roles = nameof(HubUserRole.Admin))]
    [ProducesResponseType(typeof(ClientDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateClient([FromForm] UpdateClientCommand command)
    {
        var result = await _sender.Send(command);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    /// <summary>
    /// Deletes a specific client by ID.
    /// </summary>
    /// <param name="clientId">The ID of the client to delete.</param>
    /// <returns>A success message if the client is deleted; otherwise, an error response.</returns>
    /// <remarks>
    /// This endpoint allows an admin to delete a specific client based on the provided client ID.
    /// </remarks>
    [HttpDelete("delete/{clientId:int}")]
    [Authorize(Roles = nameof(HubUserRole.Admin))]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteClient(int clientId)
    {
        var result = await _sender.Send(new DeleteClientCommand { ClientId = clientId });
        return result.IsSuccess ? Ok("Client deleted successfully") : BadRequest(result.Error);
    }
}
