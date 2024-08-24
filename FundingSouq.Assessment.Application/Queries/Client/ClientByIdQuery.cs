using FundingSouq.Assessment.Core.Dtos;
using FundingSouq.Assessment.Core.Dtos.Common;
using MediatR;

namespace FundingSouq.Assessment.Application.Queries;

/// <summary>
/// Query to retrieve a client by their ID.
/// </summary>
/// <remarks>
/// The result of the query is encapsulated in a <see cref="Result{T}"/> object. where T is a <see cref="ClientDto"/>.
/// </remarks>
public class ClientByIdQuery : IRequest<Result<ClientDto>>
{
    /// <summary>
    /// Gets or sets the ID of the client to retrieve.
    /// </summary>
    public int ClientId { get; set; }
}