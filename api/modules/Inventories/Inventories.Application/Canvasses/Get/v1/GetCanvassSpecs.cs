using Ardalis.Specification;
using AMIS.Framework.Core.Specifications;
using AMIS.WebApi.Inventories.Domain;

namespace AMIS.WebApi.Inventories.Application.Canvasses.Get.v1;

public sealed class GetCanvassSpecs : Specification<Canvass>
{
    public GetCanvassSpecs(Guid id)
    {
        Query.Where(c => c.Id == id);
    }
}

