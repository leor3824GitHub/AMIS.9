using MediatR;

namespace AMIS.WebApi.Catalog.Application.Acceptances.Cancel.v1;

public sealed record CancelAcceptanceCommand(Guid Id, string? Reason) : IRequest<CancelAcceptanceResponse>;
public sealed record CancelAcceptanceResponse(Guid Id);
