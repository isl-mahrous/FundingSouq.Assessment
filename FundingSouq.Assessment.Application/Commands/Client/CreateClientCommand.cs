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

public class CreateClientCommand : IRequest<Result<ClientDto>>
{
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
    public string AccountNumber { get; set; }
    public BankAccountType AccountType { get; set; }
}

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
        var validationResult = await ValidateRequest(request);
        if (!validationResult.IsSuccess) return validationResult.Error;

        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        var fileName = await _fileUploadService.UploadFileAsync(request.ProfilePhoto);

        var client = new Client
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            PasswordHash = _passwordHasher.Hash("client@123"),
            UserType = UserType.Client,
            PersonalId = request.PersonalId,
            ProfilePhoto = $"{_globals.CdnUrl}{fileName}",
            MobileNumber = request.MobileNumber,
            Gender = request.Gender
        };

        _unitOfWork.Clients.Add(client);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

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

        var account = new Account()
        {
            ClientId = client.Id,
            AccountNumber = request.AccountNumber,
            AccountType = request.AccountType
        };
        _unitOfWork.Accounts.Add(account);
        client.Accounts.Add(account);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await _unitOfWork.CommitTransactionAsync(cancellationToken);

        return client.Adapt<ClientDto>();
    }


    private async Task<Result> ValidateRequest(CreateClientCommand request)
    {
        var personalIdExists = await _unitOfWork.Clients.ExistsAsync(i => i.PersonalId == request.PersonalId);
        if (personalIdExists) return ClientErrors.PersonalIdInUse;

        var emailExists = await _unitOfWork.Clients.ExistsAsync(i => i.Email == request.Email);
        if (emailExists) return ClientErrors.EmailInUse;

        var mobileNumberExists = await _unitOfWork.Clients.ExistsAsync(i => i.MobileNumber == request.MobileNumber);
        if (mobileNumberExists) return ClientErrors.MobileNumberInUse;

        var accountNumberExists = await _unitOfWork.Accounts.ExistsAsync(i => i.AccountNumber == request.AccountNumber);
        if (accountNumberExists) return AccountErrors.AccountNumberExists;

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