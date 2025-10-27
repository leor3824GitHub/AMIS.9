using AMIS.Framework.Core.Exceptions;

namespace AMIS.WebApi.Catalog.Domain.Exceptions;
public sealed class AcceptanceNotFoundException : NotFoundException
{
    public AcceptanceNotFoundException(Guid id)
        : base($"Acceptance with id {id} not found")
    {
    }
}
