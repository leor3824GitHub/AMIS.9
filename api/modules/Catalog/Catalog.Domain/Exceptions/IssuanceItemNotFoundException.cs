using AMIS.Framework.Core.Exceptions;

namespace AMIS.WebApi.Catalog.Domain.Exceptions;
public sealed class IssuanceItemNotFoundException : NotFoundException
{
    public IssuanceItemNotFoundException(Guid id)
        : base($"issuanceitem with id {id} not found")
    {
    }
}
