using MediatR;

namespace AMIS.WebApi.Catalog.Application.Acceptances.Post.v1;

public sealed record PostAcceptanceCommand(Guid Id) : IRequest<PostAcceptanceResponse>;
public sealed record PostAcceptanceResponse(Guid Id);
