using AMIS.Framework.Core.Exceptions;

namespace AMIS.WebApi.Inventories.Domain.Exceptions;
public sealed class AcceptanceItemNotFoundException : NotFoundException
{
    public AcceptanceItemNotFoundException(Guid id)
        : base($"Acceptanceitem with id {id} not found")
    {
    }
}

