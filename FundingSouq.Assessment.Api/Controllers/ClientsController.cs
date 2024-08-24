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
public class ClientsController : FundingSouqControllerBase
{
    private readonly ISender _sender;

    public ClientsController(ISender sender)
    {
        _sender = sender;
    }

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
    
    [HttpGet("{clientId}")]
    [ProducesResponseType(typeof(ClientDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetClientById(int clientId)
    {
        var result = await _sender.Send(new ClientByIdQuery { ClientId = clientId });
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

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