using FundingSouq.Assessment.Core.Dtos;
using FundingSouq.Assessment.Core.Dtos.Common;
using MediatR;

namespace FundingSouq.Assessment.Application.Queries;

/// <summary>
/// Query to retrieve a list of countries, optionally filtered by a search key.
/// </summary>
/// <remarks>
/// The result of the query is encapsulated in a <see cref="Result{T}"/> object. where T is a list of <see cref="CountryDto"/> objects.
/// </remarks>
public class CountriesQuery : IRequest<Result<List<CountryDto>>>
{
    /// <summary>
    /// Gets or sets the search key used to filter countries by name.
    /// </summary>
    public string SearchKey { get; set; }
}