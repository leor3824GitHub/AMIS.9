using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Application.Suppliers.Get.v1;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Suppliers.Search.v1;
public sealed class SearchSuppliersHandler(
    [FromKeyedServices("catalog:suppliers")] IReadRepository<Supplier> repository)
    : IRequestHandler<SearchSuppliersCommand, PagedList<SupplierResponse>>
{
    public async Task<PagedList<SupplierResponse>> Handle(SearchSuppliersCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchSupplierSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<SupplierResponse>(items, request!.PageNumber, request!.PageSize, totalCount);
    }
}
