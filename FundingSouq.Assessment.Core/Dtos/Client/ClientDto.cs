using FundingSouq.Assessment.Core.Enums;

namespace FundingSouq.Assessment.Core.Dtos;

/// <summary>
/// Represents the data transfer object (DTO) for a client.
/// </summary>
/// <remarks>
/// This DTO includes client details such as personal information, profile photo, 
/// and lists of associated accounts and addresses.
/// </remarks>
public class ClientDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the client.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the email address of the client.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets the first name of the client.
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Gets or sets the last name of the client.
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Gets or sets the personal ID of the client.
    /// </summary>
    public string PersonalId { get; set; }

    /// <summary>
    /// Gets or sets the URL of the client's profile photo.
    /// </summary>
    public string ProfilePhoto { get; set; }

    /// <summary>
    /// Gets or sets the mobile number of the client.
    /// </summary>
    public string MobileNumber { get; set; }

    /// <summary>
    /// Gets or sets the gender of the client.
    /// see <see cref="Gender"/>
    /// </summary>
    public Gender Gender { get; set; }

    /// <summary>
    /// Gets the gender as a string.
    /// </summary>
    public string GenderStr => Gender.ToString();

    /// <summary>
    /// Gets or sets the list of accounts associated with the client.
    /// see <see cref="AccountDto"/>
    /// </summary>
    public List<AccountDto> Accounts { get; set; } = new List<AccountDto>();

    /// <summary>
    /// Gets or sets the list of addresses associated with the client.
    /// see <see cref="AddressDto"/>
    /// </summary>
    public List<AddressDto> Addresses { get; set; } = new List<AddressDto>();
}