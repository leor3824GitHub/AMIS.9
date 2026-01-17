using MediatR;

namespace AMIS.WebApi.Inventories.Application.Acceptances.Post.v1;

public sealed record PostAcceptanceCommand(Guid Id) : IRequest<PostAcceptanceResponse>;
public sealed record PostAcceptanceResponse(Guid Id);

