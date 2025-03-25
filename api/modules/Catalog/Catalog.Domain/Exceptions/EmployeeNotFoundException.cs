using AMIS.Framework.Core.Exceptions;

namespace AMIS.WebApi.Catalog.Domain.Exceptions;
public sealed class EmployeeNotFoundException : NotFoundException
{
    public EmployeeNotFoundException(Guid id)
        : base($"employee with id {id} not found")
    {
    }
}
