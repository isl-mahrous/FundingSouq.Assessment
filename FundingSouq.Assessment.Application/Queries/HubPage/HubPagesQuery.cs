using FundingSouq.Assessment.Core.Dtos;
using FundingSouq.Assessment.Core.Dtos.Common;
using FundingSouq.Assessment.Core.Interfaces.Repositories;
using Mapster;
using MediatR;

namespace FundingSouq.Assessment.Application.Queries;

/// <summary>
/// Query to retrieve a list of all HubPages.
/// </summary>
/// <remarks>
/// The result of the query is encapsulated in a <see cref="Result{T}"/> object. where T is a list of <see cref="HubPageDto"/> objects.
/// </remarks>
public class HubPagesQuery : IRequest<Result<List<HubPageDto>>>
{
    // No additional properties are needed for this query.
}

/// <summary>
/// Handler for processing <see cref="HubPagesQuery"/>.
/// </summary>
/// <remarks>
/// This handler retrieves a list of all HubPages from the database and maps them to a list of <see cref="HubPageDto"/> objects.
/// </remarks>
public class HubPagesQueryHandler : IRequestHandler<HubPagesQuery, Result<List<HubPageDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public HubPagesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<HubPageDto>>> Handle(HubPagesQuery request, CancellationToken cancellationToken)
    {
        // Retrieve all HubPages from the database
        var hubPages = await _unitOfWork.HubPages.GetAllAsync();
        
        // Map the list of HubPage entities to a list of HubPageDto objects and return as a result
        return hubPages.Adapt<List<HubPageDto>>();
    }
}
