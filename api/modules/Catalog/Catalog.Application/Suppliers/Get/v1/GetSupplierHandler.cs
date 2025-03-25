using Microsoft.Extensions.DependencyInjection;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using AMIS.Framework.Core.Persistence;
using AMIS.Framework.Core.Caching;
using AMIS.WebApi.Catalog.Domain;
using MediatR;

namespace AMIS.WebApi.Catalog.Application.Suppliers.Get.v1;
public sealed class GetSupplierHandler(
    [FromKeyedServices("catalog:suppliers")] IReadRepository<Supplier> repository,
    ICacheService cache)
    : IRequestHandler<GetSupplierRequest, SupplierResponse>
{
    public async Task<SupplierResponse> Handle(GetSupplierRequest request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var item = await cache.GetOrSetAsync(
            $"supplier:{request.Id}",
            async () =>
            {
                var supplierItem = await repository.GetByIdAsync(request.Id, cancellationToken);
                if (supplierItem == null) throw new SupplierNotFoundException(request.Id);
                return new SupplierResponse(supplierItem.Id, supplierItem.Name, supplierItem.Address, supplierItem.TIN, supplierItem.IsVAT, supplierItem.ContactNo, supplierItem.Emailadd);
            },
            cancellationToken: cancellationToken);
        return item!;
    }
}
