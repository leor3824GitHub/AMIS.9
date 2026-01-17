using MediatR;

namespace AMIS.WebApi.Inventories.Application.Canvasses.Create.v1;

public sealed record CreateCanvassCommand(
    Guid PurchaseRequestId,
    Guid SupplierId,
    string ItemDescription,
    int Quantity,
    string Unit,
    decimal QuotedPrice,
    string? Remarks = null,
    DateTime? ResponseDate = null
) : IRequest<CreateCanvassResponse>;

