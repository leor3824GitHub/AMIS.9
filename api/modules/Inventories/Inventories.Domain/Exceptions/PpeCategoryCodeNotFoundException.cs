using AMIS.Framework.Core.Exceptions;

namespace AMIS.WebApi.Inventories.Domain.Exceptions;

public sealed class PpeCategoryCodeNotFoundException : NotFoundException
{
    public PpeCategoryCodeNotFoundException(Guid id)
        : base($"PPE category code with id {id} not found")
    {
    }
}
