using Ardalis.Specification;
using AMIS.Framework.Core.Specifications;
using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.Canvasses.Get.v1;

public sealed class GetCanvassSpecs : Specification<Canvass>
{
    public GetCanvassSpecs(Guid id)
    {
        Query.Where(c => c.Id == id);
    }
}
