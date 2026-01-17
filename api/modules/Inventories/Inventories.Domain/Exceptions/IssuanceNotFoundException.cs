using AMIS.Framework.Core.Exceptions;

namespace AMIS.WebApi.Inventories.Domain.Exceptions;
public sealed class IssuanceNotFoundException : NotFoundException
{
    public IssuanceNotFoundException(Guid id)
        : base($"inventory with id {id} not found")
    {
    }
}

