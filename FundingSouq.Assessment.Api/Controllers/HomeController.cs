using FundingSouq.Assessment.Api.Infrastructure;
using FundingSouq.Assessment.Application.Queries;
using FundingSouq.Assessment.Core.Dtos;
using FundingSouq.Assessment.Core.Dtos.Common;
using FundingSouq.Assessment.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;

namespace FundingSouq.Assessment.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HomeController : FundingSouqControllerBase
{
    private readonly ISender _sender;

    public HomeController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("countries")]
    [OutputCache]
    [ProducesResponseType(typeof(List<CountryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCountries()
    {
        var result = await _sender.Send(new CountriesQuery());
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    [HttpGet("search-history")]
    [Authorize(Policy = nameof(UserType.HubUser))]
    [ProducesResponseType(typeof(List<SearchHistoryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetUserSearchHistory(string hubPageKey)
    {
        var result = await _sender.Send(new UserSearchHistoryQuery
            { HubUserId = GetUserId(), HubPageKey = hubPageKey });
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}