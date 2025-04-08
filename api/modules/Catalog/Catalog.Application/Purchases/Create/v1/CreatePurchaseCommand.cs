using System.ComponentModel;
using AMIS.WebApi.Catalog.Domain.ValueObjects;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.Purchases.Create.v1;
public sealed record CreatePurchaseCommand(
    Guid? SupplierId,
    DateTime? PurchaseDate,
    [property: DefaultValue(0)] decimal TotalAmount = 0,
    [property: DefaultValue("Inprogress")] string Status = "InProgress" //Pending', 'Received', 'Cancelled
    ) : IRequest<CreatePurchaseResponse>;
