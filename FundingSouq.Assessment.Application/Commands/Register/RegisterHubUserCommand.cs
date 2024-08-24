using FundingSouq.Assessment.Core.Dtos;
using FundingSouq.Assessment.Core.Dtos.Common;
using FundingSouq.Assessment.Core.Enums;
using MediatR;

namespace FundingSouq.Assessment.Application.Commands;

/// <summary>
/// Command to register a new HubUser.
/// </summary>
/// <remarks>
/// The result of the operation is encapsulated in a <see cref="Result{T}"/> object. where T is a <see cref="HubUserLoginDto"/>.
/// </remarks>
public class RegisterHubUserCommand : IRequest<Result<HubUserLoginDto>>
{
    /// <summary>
    /// Gets or sets the email of the user to be registered.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets the password of the user to be registered.
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Gets or sets the first name of the user to be registered.
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Gets or sets the last name of the user to be registered.
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Gets or sets the role of the user to be registered.
    /// </summary>
    public HubUserRole Role { get; set; }
}