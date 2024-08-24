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

    /// <summary>
    /// Retrieves a list of all countries.
    /// </summary>
    /// <returns>A list of countries.</returns>
    /// <remarks>
    /// This endpoint returns a cached list of all available countries.
    /// </remarks>
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

    /// <summary>
    /// Retrieves the search history for the authenticated hub user.
    /// </summary>
    /// <param name="hubPageKey">Optional hub page key to filter search history.</param>
    /// <returns>A list of search history records.</returns>
    /// <remarks>
    /// This endpoint returns the search history for the authenticated hub user, optionally filtered by a specific hub page.
    /// </remarks>
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

    /// <summary>
    /// Retrieves a list of hub pages.
    /// </summary>
    /// <returns>A list of hub pages.</returns>
    /// <remarks>
    /// This endpoint returns a list of all available hub pages for the authenticated hub user.
    /// </remarks>
    [HttpGet("hub-pages")]
    [Authorize(Policy = nameof(UserType.HubUser))]
    [ProducesResponseType(typeof(List<HubPageDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetHubPages()
    {
        var result = await _sender.Send(new HubPagesQuery());
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}
