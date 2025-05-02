using AMIS.Framework.Core.Exceptions;

namespace AMIS.WebApi.Catalog.Domain.Exceptions;
public sealed class InspectionNotFoundException : NotFoundException
{
    public InspectionNotFoundException(Guid id)
        : base($"inspection with id {id} not found")
    {
    }
}
