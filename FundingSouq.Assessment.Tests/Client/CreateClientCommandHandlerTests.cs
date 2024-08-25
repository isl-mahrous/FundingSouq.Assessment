using System.Linq.Expressions;
using Bogus;
using FluentAssertions;
using FundingSouq.Assessment.Application.Commands;
using FundingSouq.Assessment.Core.Dtos.Common;
using FundingSouq.Assessment.Core.Entities;
using FundingSouq.Assessment.Core.Enums;
using FundingSouq.Assessment.Core.Errors;
using FundingSouq.Assessment.Core.Interfaces;
using FundingSouq.Assessment.Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace FundingSouq.Assessment.Tests;

public class CreateClientCommandHandlerTests : UnitTestBase
{
    private readonly IUnitOfWork _unitOfWorkMock;
    private readonly IFileUploadService _fileUploadServiceMock;
    private readonly IPasswordHasher _passwordHasherMock;
    private readonly IOptions<Globals> _globalsOptionsMock;
    private readonly CreateClientCommandHandler _handler;
    private readonly Faker _faker;

    public CreateClientCommandHandlerTests()
    {
        _unitOfWorkMock = CreateMock<IUnitOfWork>();
        _fileUploadServiceMock = CreateMock<IFileUploadService>();
        _passwordHasherMock = CreateMock<IPasswordHasher>();

        var globals = new Globals
        {
            CdnUrl = "https://testcdn.com/"
        };

        _globalsOptionsMock = CreateOptions(globals);
        _handler = new CreateClientCommandHandler(
            _unitOfWorkMock,
            _fileUploadServiceMock,
            _passwordHasherMock,
            _globalsOptionsMock);

        _faker = new Faker();
    }

    [Fact]
    public async Task Handle_Should_Return_Success_When_Client_Is_Created()
    {
        // Arrange
        var command = CreateValidCommand();

        SetupUnitOfWorkMocksForSuccess();

        _fileUploadServiceMock.UploadFileAsync(Arg.Any<IFormFile>())
            .Returns(Task.FromResult("profile-photo.png"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Email.Should().Be(command.Email);
        result.Value.ProfilePhoto.Should().Be($"{_globalsOptionsMock.Value.CdnUrl}profile-photo.png");
    }

    [Fact]
    public async Task Handle_Should_Fail_When_PersonalId_Is_Duplicate()
    {
        // Arrange
        var command = CreateValidCommand();

        // Mock the checks that should pass
        _unitOfWorkMock.Clients.ExistsAsync(Arg.Is<Expression<Func<Client, bool>>>(expr =>
            expr.Compile().Invoke(new Client { PersonalId = command.PersonalId }) == false)).Returns(false);

        _unitOfWorkMock.Clients.ExistsAsync(Arg.Is<Expression<Func<Client, bool>>>(expr =>
            expr.Compile().Invoke(new Client { Email = command.Email }) == true)).Returns(false);

        _unitOfWorkMock.Clients.ExistsAsync(Arg.Is<Expression<Func<Client, bool>>>(expr =>
            expr.Compile().Invoke(new Client { MobileNumber = command.MobileNumber }) == true)).Returns(false);

        _unitOfWorkMock.Accounts.ExistsAsync(Arg.Is<Expression<Func<Account, bool>>>(expr =>
            expr.Compile().Invoke(new Account { AccountNumber = command.AccountNumber }) == true)).Returns(false);

        _unitOfWorkMock.Cities.ExistsAsync(Arg.Any<Expression<Func<City, bool>>>())
            .Returns(Task.FromResult(true));

        _unitOfWorkMock.Countries.ExistsAsync(Arg.Any<Expression<Func<Country, bool>>>())
            .Returns(Task.FromResult(true));

        _unitOfWorkMock.Cities.ExistsAsync(Arg.Any<Expression<Func<City, bool>>>())
            .Returns(Task.FromResult(true));

        // Mock the PersonalId check to simulate failure due to duplicate PersonalId
        _unitOfWorkMock.Clients.ExistsAsync(Arg.Is<Expression<Func<Client, bool>>>(expr =>
            expr.Compile().Invoke(new Client { PersonalId = command.PersonalId }) == true)).Returns(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be("PERSONAL_ID_IN_USE");
        result.Error.Message.Should().Be("Personal ID is already in use.");
    }


    [Fact]
    public async Task Handle_Should_Fail_When_Email_Is_Duplicate()
    {
        // Arrange
        var command = CreateValidCommand();

        // Mock successful validations for other checks
        SetupUnitOfWorkMocksForSuccess();

        // Mock the email duplication check to return true, causing failure
        _unitOfWorkMock.Clients.ExistsAsync(Arg.Is<Expression<Func<Client, bool>>>(expr =>
                expr.Compile().Invoke(new Client { Email = command.Email })))
            .Returns(Task.FromResult(true));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be(ClientErrors.EmailInUse.Code);
        result.Error.Message.Should().Be(ClientErrors.EmailInUse.Message);
    }


    [Fact]
    public async Task Handle_Should_Fail_When_MobileNumber_Is_Duplicate()
    {
        // Arrange
        var command = CreateValidCommand();

        // Mock successful validations for other checks
        SetupUnitOfWorkMocksForSuccess();

        // Mock the mobile number duplication check to return true, causing failure
        _unitOfWorkMock.Clients.ExistsAsync(Arg.Is<Expression<Func<Client, bool>>>(expr =>
                expr.Compile().Invoke(new Client { MobileNumber = command.MobileNumber })))
            .Returns(Task.FromResult(true));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be(ClientErrors.MobileNumberInUse.Code);
        result.Error.Message.Should().Be(ClientErrors.MobileNumberInUse.Message);
    }


    [Fact]
    public async Task Handle_Should_Fail_When_AccountNumber_Is_Duplicate()
    {
        // Arrange
        var command = CreateValidCommand();

        // Mock successful validations for other checks
        SetupUnitOfWorkMocksForSuccess();

        // Mock the account number duplication check to return true, causing failure
        _unitOfWorkMock.Accounts.ExistsAsync(Arg.Is<Expression<Func<Account, bool>>>(expr =>
                expr.Compile().Invoke(new Account { AccountNumber = command.AccountNumber })))
            .Returns(Task.FromResult(true));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be(AccountErrors.AccountNumberExists.Code);
        result.Error.Message.Should().Be(AccountErrors.AccountNumberExists.Message);
    }


    [Fact]
    public async Task Handle_Should_Fail_When_City_Is_Not_Found()
    {
        // Arrange
        var command = CreateValidCommand();

        // Mock successful validations for other checks
        SetupUnitOfWorkMocksForSuccess();

        // Mock the city existence check to return false, causing failure
        _unitOfWorkMock.Cities.ExistsAsync(Arg.Is<Expression<Func<City, bool>>>(expr =>
                expr.Compile().Invoke(new City { Id = command.CityId })))
            .Returns(Task.FromResult(false));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be(ClientErrors.CityNotFound.Code);
        result.Error.Message.Should().Be(ClientErrors.CityNotFound.Message);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_Country_Is_Not_Found()
    {
        // Arrange
        var command = CreateValidCommand();

        // Mock successful validations for other checks
        SetupUnitOfWorkMocksForSuccess();

        // Mock the country existence check to return false, causing failure
        _unitOfWorkMock.Countries.ExistsAsync(Arg.Is<Expression<Func<Country, bool>>>(expr =>
                expr.Compile().Invoke(new Country { Id = command.CountryId })))
            .Returns(Task.FromResult(false));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Code.Should().Be(ClientErrors.CountryNotFound.Code);
        result.Error.Message.Should().Be(ClientErrors.CountryNotFound.Message);
    }
    
    private void SetupUnitOfWorkMocksForSuccess(bool setupCity = false, bool setupCountry = false)
    {
        _unitOfWorkMock.Clients.ExistsAsync(Arg.Any<Expression<Func<Client, bool>>>()).Returns(Task.FromResult(false));
        _unitOfWorkMock.Accounts.ExistsAsync(Arg.Any<Expression<Func<Account, bool>>>()).Returns(Task.FromResult(false));

        if (!setupCity)
        {
            _unitOfWorkMock.Cities.ExistsAsync(Arg.Any<Expression<Func<City, bool>>>()).Returns(Task.FromResult(true));
        }

        if (!setupCountry)
        {
            _unitOfWorkMock.Countries.ExistsAsync(Arg.Any<Expression<Func<Country, bool>>>()).Returns(Task.FromResult(true));
        }
    }


    private CreateClientCommand CreateValidCommand()
    {
        return new CreateClientCommand
        {
            Email = _faker.Internet.Email(),
            FirstName = _faker.Name.FirstName(),
            LastName = _faker.Name.LastName(),
            PersonalId = _faker.Random.String2(11, "0123456789"),
            MobileNumber = _faker.Phone.PhoneNumber("+###########"),
            Gender = Gender.Male,
            CountryId = 1,
            CityId = 1,
            Street = _faker.Address.StreetAddress(),
            ZipCode = _faker.Address.ZipCode(),
            AccountNumber = _faker.Finance.Account()
        };
    }
}