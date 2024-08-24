using FundingSouq.Assessment.Core.Dtos;
using FundingSouq.Assessment.Core.Dtos.Common;
using FundingSouq.Assessment.Core.Interfaces.Repositories;
using Mapster;
using MediatR;

namespace FundingSouq.Assessment.Application.Queries;

public class HubPagesQuery : IRequest<Result<List<HubPageDto>>>
{
    
}

public class HubPagesQueryHandler : IRequestHandler<HubPagesQuery, Result<List<HubPageDto>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public HubPagesQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result<List<HubPageDto>>> Handle(HubPagesQuery request, CancellationToken cancellationToken)
    {
        var hubPages = await _unitOfWork.HubPages.GetAllAsync();
        return hubPages.Adapt<List<HubPageDto>>();
    }
}