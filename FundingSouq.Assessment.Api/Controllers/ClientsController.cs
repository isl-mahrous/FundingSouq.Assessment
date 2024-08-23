using FundingSouq.Assessment.Api.Infrastructure;
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
}