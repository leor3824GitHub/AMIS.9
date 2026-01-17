using AMIS.Framework.Core.Exceptions;

namespace AMIS.WebApi.Inventories.Domain.Exceptions;
public sealed class InventoryNotFoundException : NotFoundException
{
    public InventoryNotFoundException(Guid id)
        : base($"issuance with id {id} not found")
    {
    }
}

