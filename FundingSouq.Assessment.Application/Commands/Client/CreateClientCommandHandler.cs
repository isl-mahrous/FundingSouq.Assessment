using FundingSouq.Assessment.Core.Dtos;
using FundingSouq.Assessment.Core.Dtos.Common;
using FundingSouq.Assessment.Core.Entities;
using FundingSouq.Assessment.Core.Enums;
using FundingSouq.Assessment.Core.Errors;
using FundingSouq.Assessment.Core.Interfaces;
using FundingSouq.Assessment.Core.Interfaces.Repositories;
using Mapster;
using MediatR;
using Microsoft.Extensions.Options;

namespace FundingSouq.Assessment.Application.Commands;

/// <summary>
/// Handler for processing <see cref="CreateClientCommand"/>.
/// </summary>
/// <remarks>
/// This handler creates a new client, uploads the profile photo, and associates the client with an address and account.
/// </remarks>
public class CreateClientCommandHandler : IRequestHandler<CreateClientCommand, Result<ClientDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IFileUploadService _fileUploadService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly Globals _globals;

    public CreateClientCommandHandler(
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

    public async Task<Result<ClientDto>> Handle(CreateClientCommand request, CancellationToken cancellationToken)
    {
        // Validate the request data
        var validationResult = await ValidateRequest(request);
        if (!validationResult.IsSuccess) return validationResult.Error;

        // Start a new transaction
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        // Upload the profile photo and get the file name
        var fileName = await _fileUploadService.UploadFileAsync(request.ProfilePhoto);

        // Create a new client entity and set its properties
        var client = new Client
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PasswordHash = _passwordHasher.Hash("client@123"), // Default password
            UserType = UserType.Client,
            PersonalId = request.PersonalId,
            ProfilePhoto = $"{_globals.CdnUrl}{fileName}",
            MobileNumber = request.MobileNumber,
            Gender = request.Gender
        };

        // Add the client to the repository
        _unitOfWork.Clients.Add(client);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Create a new address entity and associate it with the client
        var address = new Address()
        {
            ClientId = client.Id,
            CountryId = request.CountryId,
            CityId = request.CityId,
            Street = request.Street,
            ZipCode = request.ZipCode
        };
        _unitOfWork.Addresses.Add(address);
        client.Addresses.Add(address);

        // Create a new account entity and associate it with the client
        var account = new Account()
        {
            ClientId = client.Id,
            AccountNumber = request.AccountNumber,
            AccountType = request.AccountType
        };
        _unitOfWork.Accounts.Add(account);
        client.Accounts.Add(account);

        // Save all changes and commit the transaction
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _unitOfWork.CommitTransactionAsync(cancellationToken);

        // Return the created client as a DTO
        return client.Adapt<ClientDto>();
    }

    /// <summary>
    /// Validates the <see cref="CreateClientCommand"/> request.
    /// </summary>
    /// <param name="request">The request object containing client data.</param>
    /// <returns>A <see cref="Result"/> indicating success or failure based on validation checks.</returns>
    private async Task<Result> ValidateRequest(CreateClientCommand request)
    {
        // Check if the personal ID already exists
        var personalIdExists = await _unitOfWork.Clients.ExistsAsync(i => i.PersonalId == request.PersonalId);
        if (personalIdExists) return ClientErrors.PersonalIdInUse;

        // Check if the email already exists
        var emailExists = await _unitOfWork.Clients.ExistsAsync(i => i.Email == request.Email);
        if (emailExists) return ClientErrors.EmailInUse;

        // Check if the mobile number already exists
        var mobileNumberExists = await _unitOfWork.Clients.ExistsAsync(i => i.MobileNumber == request.MobileNumber);
        if (mobileNumberExists) return ClientErrors.MobileNumberInUse;

        // Check if the account number already exists
        var accountNumberExists = await _unitOfWork.Accounts.ExistsAsync(i => i.AccountNumber == request.AccountNumber);
        if (accountNumberExists) return AccountErrors.AccountNumberExists;

        // Check if the city exists
        var cityExists = await _unitOfWork.Cities.ExistsAsync(i => i.Id == request.CityId);
        if (!cityExists) return ClientErrors.CityNotFound;

        // Check if the country exists
        var countryExists = await _unitOfWork.Countries.ExistsAsync(i => i.Id == request.CountryId);
        if (!countryExists) return ClientErrors.CountryNotFound;

        // Check if the city belongs to the specified country
        var cityBelongsToCountry = await _unitOfWork.Cities
            .ExistsAsync(i => i.Id == request.CityId && i.CountryId == request.CountryId);
        if (!cityBelongsToCountry) return ClientErrors.CityNotBelongToCountry;

        return Result.Success();
    }
}