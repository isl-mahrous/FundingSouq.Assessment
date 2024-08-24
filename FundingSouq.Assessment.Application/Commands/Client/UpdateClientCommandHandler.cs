using FundingSouq.Assessment.Core.Dtos;
using FundingSouq.Assessment.Core.Dtos.Common;
using FundingSouq.Assessment.Core.Entities;
using FundingSouq.Assessment.Core.Errors;
using FundingSouq.Assessment.Core.Interfaces;
using FundingSouq.Assessment.Core.Interfaces.Repositories;
using Mapster;
using MediatR;
using Microsoft.Extensions.Options;

namespace FundingSouq.Assessment.Application.Commands;

/// <summary>
/// Handler for processing <see cref="UpdateClientCommand"/>.
/// </summary>
/// <remarks>
/// This handler updates the client's details and related address information.
/// If a profile photo is provided, it is uploaded, and the client's profile photo URL is updated accordingly.
/// Changes are saved within a transaction to ensure data integrity.
/// </remarks>
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

        // Fetch the existing client
        var client = await _unitOfWork.Clients.GetFirstAsync(c => c.Id == request.ClientId);
        if (client == null) return ClientErrors.ClientNotFound;

        // Update profile photo if provided
        if (request.ProfilePhoto != null)
        {
            var fileName = await _fileUploadService.UploadFileAsync(request.ProfilePhoto);
            client.ProfilePhoto = $"{_globals.CdnUrl}{fileName}";
        }

        // Update client details
        client.Email = request.Email;
        client.FirstName = request.FirstName;
        client.LastName = request.LastName;
        client.PersonalId = request.PersonalId;
        client.MobileNumber = request.MobileNumber;
        client.Gender = request.Gender;

        // Update or create the client's address
        var address = await _unitOfWork.Addresses.GetFirstAsync(a => a.ClientId == client.Id);
        if (address == null)
        {
            address = new Address
            {
                ClientId = client.Id
            };
            _unitOfWork.Addresses.Add(address);
        }
        
        // Update address details
        address.CountryId = request.CountryId;
        address.CityId = request.CityId;
        address.Street = request.Street;
        address.ZipCode = request.ZipCode;

        // Save changes and commit the transaction
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _unitOfWork.CommitTransactionAsync(cancellationToken);
        
        return client.Adapt<ClientDto>();
    }

    /// <summary>
    /// Validates the incoming request to ensure the provided data is valid.
    /// </summary>
    /// <param name="request">The update client request to validate.</param>
    /// <returns>A <see cref="Result"/> indicating the success or failure of the validation.</returns>
    private async Task<Result> ValidateRequest(UpdateClientCommand request)
    {
        // Ensure the client exists
        var clientExists = await _unitOfWork.Clients.ExistsAsync(c => c.Id == request.ClientId);
        if (!clientExists) return ClientErrors.ClientNotFound;

        // Check for existing email, personal ID, or mobile number conflicts
        var emailExists = await _unitOfWork.Clients.ExistsAsync(i => i.Email == request.Email && i.Id != request.ClientId);
        if (emailExists) return ClientErrors.EmailInUse;

        var personalIdExists = await _unitOfWork.Clients.ExistsAsync(i => i.PersonalId == request.PersonalId && i.Id != request.ClientId);
        if (personalIdExists) return ClientErrors.PersonalIdInUse;

        var mobileNumberExists = await _unitOfWork.Clients.ExistsAsync(i => i.MobileNumber == request.MobileNumber && i.Id != request.ClientId);
        if (mobileNumberExists) return ClientErrors.MobileNumberInUse;

        // Validate city and country
        var cityExists = await _unitOfWork.Cities.ExistsAsync(i => i.Id == request.CityId);
        if (!cityExists) return ClientErrors.CityNotFound;

        var countryExists = await _unitOfWork.Countries.ExistsAsync(i => i.Id == request.CountryId);
        if (!countryExists) return ClientErrors.CountryNotFound;
        
        // Ensure the city belongs to the specified country
        var cityBelongsToCountry = await _unitOfWork.Cities
            .ExistsAsync(i => i.Id == request.CityId && i.CountryId == request.CountryId);
        if (!cityBelongsToCountry) return ClientErrors.CityNotBelongToCountry;
        
        return Result.Success();
    }
}