using FundingSouq.Assessment.Core.Dtos;
using FundingSouq.Assessment.Core.Dtos.Common;
using MediatR;

namespace FundingSouq.Assessment.Application.Commands;

/// <summary>
/// Command to log in a HubUser.
/// </summary>
/// <remarks>
/// The result of the operation is encapsulated in a <see cref="Result{T}"/> object. Where T is a <see cref="HubUserLoginDto"/>.
/// </remarks>
public class LoginHubUserCommand : IRequest<Result<HubUserLoginDto>>
{
    /// <summary>
    /// Gets or sets the email of the user attempting to log in.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets the password of the user attempting to log in.
    /// </summary>
    public string Password { get; set; }
}