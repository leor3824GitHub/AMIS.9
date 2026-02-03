using AMIS.Framework.Core.Exceptions;

namespace AMIS.WebApi.Inventories.Domain.Exceptions;

public sealed class NfaOfficeCodeNotFoundException : NotFoundException
{
    public NfaOfficeCodeNotFoundException(Guid id)
        : base($"NFA office code with id {id} not found")
    {
    }
}
