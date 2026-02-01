using MediatR;

namespace AMIS.WebApi.Inventories.Application.PropertyAcknowledgementReceipt.Post.v1;

public sealed record PostPARCommand(Guid Id) : IRequest<PostPARResponse>;

public sealed record PostPARResponse(
    Guid Id,
    string PARNumber,
    string Status,
    DateTime PostedAt);
