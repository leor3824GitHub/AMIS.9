using MediatR;

namespace AMIS.WebApi.Catalog.Application.Canvasses.Select.v1;

public sealed record SelectLowestCanvassCommand(Guid PurchaseRequestId) : IRequest<SelectLowestCanvassResponse>;

public sealed record SelectLowestCanvassResponse(Guid SelectedCanvassId, decimal QuotedPrice);
