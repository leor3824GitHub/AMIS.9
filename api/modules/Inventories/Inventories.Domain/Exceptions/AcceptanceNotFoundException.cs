using AMIS.Framework.Core.Exceptions;

namespace AMIS.WebApi.Inventories.Domain.Exceptions;
public sealed class AcceptanceNotFoundException : NotFoundException
{
    public AcceptanceNotFoundException(Guid id)
        : base($"Acceptance with id {id} not found")
    {
    }
}

