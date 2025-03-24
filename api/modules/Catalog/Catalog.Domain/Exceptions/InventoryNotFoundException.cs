using AMIS.Framework.Core.Exceptions;

namespace AMIS.WebApi.Catalog.Domain.Exceptions;
public sealed class InventoryNotFoundException : NotFoundException
{
    public InventoryNotFoundException(Guid id)
        : base($"inventory with id {id} not found")
    {
    }
}
