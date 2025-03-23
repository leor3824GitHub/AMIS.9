using AMIS.Framework.Core.Exceptions;

namespace AMIS.WebApi.Catalog.Domain.Exceptions;
public sealed class CategoryNotFoundException : NotFoundException
{
    public CategoryNotFoundException(Guid id)
        : base($"category with id {id} not found")
    {
    }
}
