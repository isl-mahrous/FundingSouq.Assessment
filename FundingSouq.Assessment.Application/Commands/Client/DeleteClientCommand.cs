using FundingSouq.Assessment.Core.Dtos.Common;
using MediatR;

namespace FundingSouq.Assessment.Application.Commands;

/// <summary>
/// Command to delete a client.
/// </summary>
/// <remarks>
/// The result of the operation is encapsulated in a <see cref="Result"/> object.
/// </remarks>
public class DeleteClientCommand : IRequest<Result>
{
    /// <summary>
    /// Gets or sets the ID of the client to be deleted.
    /// </summary>
    public int ClientId { get; set; }
}