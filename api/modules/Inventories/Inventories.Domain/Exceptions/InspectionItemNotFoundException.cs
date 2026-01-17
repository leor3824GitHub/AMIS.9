using AMIS.Framework.Core.Exceptions;

namespace AMIS.WebApi.Inventories.Domain.Exceptions;
public sealed class InspectionItemNotFoundException : NotFoundException
{
    public InspectionItemNotFoundException(Guid id)
        : base($"inspectionitem with id {id} not found")
    {
    }
}

