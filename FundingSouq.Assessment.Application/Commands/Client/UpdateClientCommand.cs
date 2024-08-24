using FundingSouq.Assessment.Core.Dtos;
using FundingSouq.Assessment.Core.Dtos.Common;
using FundingSouq.Assessment.Core.Entities;
using FundingSouq.Assessment.Core.Enums;
using FundingSouq.Assessment.Core.Errors;
using FundingSouq.Assessment.Core.Interfaces;
using FundingSouq.Assessment.Core.Interfaces.Repositories;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace FundingSouq.Assessment.Application.Commands;

public class UpdateClientCommand : IRequest<Result<ClientDto>>
{
    public int ClientId { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PersonalId { get; set; }
    public IFormFile ProfilePhoto { get; set; }
    public string MobileNumber { get; set; }
    public Gender Gender { get; set; }
    public int CountryId { get; set; }
    public int CityId { get; set; }
    public string Street { get; set; }
    public string ZipCode { get; set; }
}

public class UpdateClientCommandHandler : IRequestHandler<UpdateClientCommand, Result<ClientDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileUploadService _fileUploadService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly Globals _globals;

    public UpdateClientCommandHandler(
        IUnitOfWork unitOfWork,
        IFileUploadService fileUploadService,
        IPasswordHasher passwordHasher,
        IOptions<Globals> options)
    {
        _unitOfWork = unitOfWork;
        _fileUploadService = fileUploadService;
        _passwordHasher = passwordHasher;
        _globals = options.Value;
    }

    public async Task<Result<ClientDto>> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
    {
        var validationResult = await ValidateRequest(request);
        if (!validationResult.IsSuccess) return validationResult.Error;

        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        var client = await _unitOfWork.Clients.GetFirstAsync(c => c.Id == request.ClientId);
        if (client == null) return ClientErrors.ClientNotFound;

        if (request.ProfilePhoto != null)
        {
            var fileName = await _fileUploadService.UploadFileAsync(request.ProfilePhoto);
            client.ProfilePhoto = $"{_globals.CdnUrl}{fileName}";
        }

        client.Email = request.Email;
        client.FirstName = request.FirstName;
        client.LastName = request.LastName;
        client.PersonalId = request.PersonalId;
        client.MobileNumber = request.MobileNumber;
        client.Gender = request.Gender;

        var address = await _unitOfWork.Addresses.GetFirstAsync(a => a.ClientId == client.Id);
        if (address is null)
        {
            address = new Address
            {
                ClientId = client.Id
            };
            _unitOfWork.Addresses.Add(address);
        }
        
        address.CountryId = request.CountryId;
        address.CityId = request.CityId;
        address.Street = request.Street;
        address.ZipCode = request.ZipCode;

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _unitOfWork.CommitTransactionAsync(cancellationToken);
        
        return client.Adapt<ClientDto>();
    }

    private async Task<Result> ValidateRequest(UpdateClientCommand request)
    {
        var clientExists = await _unitOfWork.Clients.ExistsAsync(c => c.Id == request.ClientId);
        if (!clientExists) return ClientErrors.ClientNotFound;

        var emailExists = await _unitOfWork.Clients.ExistsAsync(i => i.Email == request.Email && i.Id != request.ClientId);
        if (emailExists) return ClientErrors.EmailInUse;

        var personalIdExists = await _unitOfWork.Clients.ExistsAsync(i => i.PersonalId == request.PersonalId && i.Id != request.ClientId);
        if (personalIdExists) return ClientErrors.PersonalIdInUse;

        var mobileNumberExists = await _unitOfWork.Clients.ExistsAsync(i => i.MobileNumber == request.MobileNumber && i.Id != request.ClientId);
        if (mobileNumberExists) return ClientErrors.MobileNumberInUse;

        var cityExists = await _unitOfWork.Cities.ExistsAsync(i => i.Id == request.CityId);
        if (!cityExists) return ClientErrors.CityNotFound;

        var countryExists = await _unitOfWork.Countries.ExistsAsync(i => i.Id == request.CountryId);
        if (!countryExists) return ClientErrors.CountryNotFound;
        
        var cityBelongsToCountry = await _unitOfWork.Cities
            .ExistsAsync(i => i.Id == request.CityId && i.CountryId == request.CountryId);
        if (!cityBelongsToCountry) return ClientErrors.CityNotBelongToCountry;
        
        return Result.Success();
    }
}