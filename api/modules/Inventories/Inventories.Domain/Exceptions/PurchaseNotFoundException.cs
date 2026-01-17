using AMIS.Framework.Core.Exceptions;

namespace AMIS.WebApi.Inventories.Domain.Exceptions;
public sealed class PurchaseNotFoundException : NotFoundException
{
    public PurchaseNotFoundException(Guid id)
        : base($"purchase with id {id} not found")
    {
    }
}

