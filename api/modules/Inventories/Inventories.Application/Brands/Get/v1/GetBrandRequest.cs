using MediatR;

namespace AMIS.WebApi.Inventories.Application.Brands.Get.v1;
public class GetBrandRequest : IRequest<BrandResponse>
{
    public Guid Id { get; set; }
    public GetBrandRequest(Guid id) => Id = id;
}

