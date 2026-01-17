using AMIS.Framework.Core.Exceptions;

namespace AMIS.WebApi.Inventories.Domain.Exceptions;
public sealed class PurchaseItemNotFoundException : NotFoundException
{
    public PurchaseItemNotFoundException(Guid id)
        : base($"purchaseitem with id {id} not found")
    {
    }
}

