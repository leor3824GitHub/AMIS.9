using MediatR;

namespace AMIS.WebApi.Inventories.Application.Canvasses.Award.v1;

public sealed record AwardCanvassCommand(Guid Id) : IRequest<AwardCanvassResponse>;

public sealed record AwardCanvassResponse(Guid AwardedCanvassId, Guid SupplierId, decimal QuotedPrice);

