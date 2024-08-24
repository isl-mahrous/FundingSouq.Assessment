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
    /// <param name="sortDirection">Direction to sort the clients. Ascending = 0 and Descending = 1</param>
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
    /// The request must include all necessary client information, including:
    /// <list type="bullet">
    /// <item>
    /// <description><b>Email</b>: The client's email address. Must be valid and unique.</description>
    /// </item>
    /// <item>
    /// <description><b>FirstName</b>: The client's first name. Maximum length is 60 characters.</description>
    /// </item>
    /// <item>
    /// <description><b>LastName</b>: The client's last name. Maximum length is 60 characters.</description>
    /// </item>
    /// <item>
    /// <description><b>PersonalId</b>: The client's personal ID (e.g., national ID). Must be exactly 11 characters.</description>
    /// </item>
    /// <item>
    /// <description><b>ProfilePhoto</b>: The client's profile photo. Must be a valid file.</description>
    /// </item>
    /// <item>
    /// <description><b>MobileNumber</b>: The client's mobile number. Must be in international format (e.g., +971501234567) and unique.</description>
    /// </item>
    /// <item>
    /// <description><b>Gender</b>: The client's gender. Accepted values are 0 for Male and 1 for Female.</description>
    /// </item>
    /// <item>
    /// <description><b>CountryId</b>: The ID of the country where the client resides. Must be a valid country ID.</description>
    /// </item>
    /// <item>
    /// <description><b>CityId</b>: The ID of the city where the client resides. Must be a valid city ID associated with the specified country.</description>
    /// </item>
    /// <item>
    /// <description><b>Street</b>: The street address of the client. Cannot be empty.</description>
    /// </item>
    /// <item>
    /// <description><b>ZipCode</b>: The zip code of the client's address. Cannot be empty.</description>
    /// </item>
    /// <item>
    /// <description><b>AccountNumber</b>: The client's account number. Must be unique and cannot be empty.</description>
    /// </item>
    /// <item>
    /// <description><b>AccountType</b>: The type of the client's account. Valid values are between 0 and 10.</description>
    /// </item>
    /// </list>
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