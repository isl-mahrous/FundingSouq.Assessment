using FundingSouq.Assessment.Core.Dtos.Common;
using MediatR;

namespace FundingSouq.Assessment.Application.Commands;

/// <summary>
/// Command to delete a client's account.
/// </summary>
/// <remarks>
/// This command is used to delete a specific client account identified by its Id.
/// The result of the operation is encapsulated in a <see cref="Result"/> object.
/// </remarks>
public class DeleteClientAccountCommand : IRequest<Result>
{
    /// <summary>
    /// Gets or sets the Id of the account to be deleted.
    /// </summary>
    public int Id { get; set; }
}