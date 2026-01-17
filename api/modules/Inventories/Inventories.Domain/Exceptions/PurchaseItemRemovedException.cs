using AMIS.Framework.Core.Exceptions;

namespace AMIS.WebApi.Inventories.Domain.Exceptions;
public sealed class PurchaseItemRemovedException : NotFoundException
{
    public PurchaseItemRemovedException(Guid id)
        : base($"purchaseitem with id {id} not found")
    {
    }
}

