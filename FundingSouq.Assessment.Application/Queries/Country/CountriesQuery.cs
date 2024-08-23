using FundingSouq.Assessment.Core.Dtos;
using FundingSouq.Assessment.Core.Dtos.Common;
using FundingSouq.Assessment.Core.Interfaces.Repositories;
using Mapster;
using MediatR;

namespace FundingSouq.Assessment.Application.Queries;

public class CountriesQuery : IRequest<Result<List<CountryDto>>>
{
    public string SearchKey { get; set; }
}

public class CountryQueryHandler : IRequestHandler<CountriesQuery, Result<List<CountryDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public CountryQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<CountryDto>>> Handle(CountriesQuery request, CancellationToken cancellationToken)
    {
        var countries = await _unitOfWork.Countries.GetAllAsync(
                predicate: x => string.IsNullOrEmpty(request.SearchKey) || x.Name.Contains(request.SearchKey),
                includes: c => c.Cities);

        return countries.Adapt<List<CountryDto>>();
    }
}