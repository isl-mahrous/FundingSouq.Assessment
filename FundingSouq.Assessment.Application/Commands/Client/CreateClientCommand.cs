using FundingSouq.Assessment.Core.Dtos;
using FundingSouq.Assessment.Core.Dtos.Common;
using FundingSouq.Assessment.Core.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace FundingSouq.Assessment.Application.Commands;

/// <summary>
/// Command to create a new client with associated account and address details.
/// </summary>
/// <remarks>
/// The result of the operation is encapsulated in a <see cref="Result{T}"/> object, where T is <see cref="ClientDto"/>.
/// </remarks>
public class CreateClientCommand : IRequest<Result<ClientDto>>
{
    /// <summary>
    /// Gets or sets the client's email address.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets the client's first name.
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Gets or sets the client's last name.
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Gets or sets the client's personal ID (e.g., national ID).
    /// </summary>
    public string PersonalId { get; set; }

    /// <summary>
    /// Gets or sets the client's profile photo as an IFormFile.
    /// </summary>
    public IFormFile ProfilePhoto { get; set; }

    /// <summary>
    /// Gets or sets the client's mobile number.
    /// </summary>
    public string MobileNumber { get; set; }

    /// <summary>
    /// Gets or sets the client's gender.
    /// </summary>
    public Gender Gender { get; set; }

    /// <summary>
    /// Gets or sets the ID of the country where the client resides.
    /// </summary>
    public int CountryId { get; set; }

    /// <summary>
    /// Gets or sets the ID of the city where the client resides.
    /// </summary>
    public int CityId { get; set; }

    /// <summary>
    /// Gets or sets the street address of the client.
    /// </summary>
    public string Street { get; set; }

    /// <summary>
    /// Gets or sets the zip code of the client's address.
    /// </summary>
    public string ZipCode { get; set; }

    /// <summary>
    /// Gets or sets the client's account number.
    /// </summary>
    public string AccountNumber { get; set; }

    /// <summary>
    /// Gets or sets the type of the client's account.
    /// </summary>
    public BankAccountType AccountType { get; set; }
}