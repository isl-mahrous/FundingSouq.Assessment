using FundingSouq.Assessment.Core.Dtos;
using FundingSouq.Assessment.Core.Dtos.Common;
using FundingSouq.Assessment.Core.Interfaces.Repositories;
using Mapster;
using MediatR;

namespace FundingSouq.Assessment.Application.Queries;

/// <summary>
/// Handler for processing <see cref="CountriesQuery"/>.
/// </summary>
/// <remarks>
/// This handler retrieves a list of countries from the database, optionally filtering them by the provided search key.
/// The result includes associated cities for each country.
/// </remarks>
public class CountryQueryHandler : IRequestHandler<CountriesQuery, Result<List<CountryDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CountryQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<CountryDto>>> Handle(CountriesQuery request, CancellationToken cancellationToken)
    {
        // Retrieve countries from the database, including their associated cities
        // Apply filtering if a search key is provided
        var countries = await _unitOfWork.Countries.GetAllAsync(
            predicate: x => string.IsNullOrEmpty(request.SearchKey) || x.Name.Contains(request.SearchKey),
            includes: c => c.Cities);

        // Map the list of Country entities to a list of CountryDto objects and return as a result
        return countries.Adapt<List<CountryDto>>();
    }
}