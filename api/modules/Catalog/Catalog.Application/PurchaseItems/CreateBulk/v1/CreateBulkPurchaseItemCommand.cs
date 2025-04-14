using AMIS.WebApi.Catalog.Application.PurchaseItems.Create.v1;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.PurchaseItems.CreateBulk.v1;
public sealed record CreateBulkPurchaseItemCommand(
    Guid PurchaseId,
    List<CreatePurchaseItemCommand> Items
) : IRequest<CreateBulkPurchaseItemResponse>;
