using MediatR;

namespace AMIS.WebApi.Inventories.Application.Suppliers.Get.v1;
public class GetSupplierRequest : IRequest<SupplierResponse>
{
    public Guid Id { get; set; }
    public GetSupplierRequest(Guid id) => Id = id;
}

