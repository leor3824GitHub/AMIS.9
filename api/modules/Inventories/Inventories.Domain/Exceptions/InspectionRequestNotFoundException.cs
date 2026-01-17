using AMIS.Framework.Core.Exceptions;

namespace AMIS.WebApi.Inventories.Domain.Exceptions;
public sealed class InspectionRequestNotFoundException : NotFoundException
{
    public InspectionRequestNotFoundException(Guid id)
        : base($"inspection request with id {id} not found")
    {
    }
}

