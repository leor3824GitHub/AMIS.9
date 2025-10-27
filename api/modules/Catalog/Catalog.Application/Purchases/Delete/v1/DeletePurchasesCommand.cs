using MediatR;
using System.Collections.Generic;

namespace AMIS.WebApi.Catalog.Application.Purchases.Delete.v1
{
    public record DeletePurchasesCommand(IEnumerable<Guid> PurchaseIds) : IRequest;
}
