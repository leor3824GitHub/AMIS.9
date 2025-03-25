using System.ComponentModel;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.Purchases.Create.v1;
public sealed record CreatePurchaseCommand(
    Guid SupplierId,
    DateTime PurchaseDate,
    [property: DefaultValue(0)] decimal TotalAmount = 0,
    [property: DefaultValue("Pending")] string? Status = "Pending" //Pending', 'Received', 'Cancelled
    ) : IRequest<CreatePurchaseResponse>;
