using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Application.Employees.Get.v1;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Employees.Search.v1;
public sealed class SearchEmployeesHandler(
    [FromKeyedServices("catalog:employees")] IReadRepository<Employee> repository)
    : IRequestHandler<SearchEmployeesCommand, PagedList<EmployeeResponse>>
{
    public async Task<PagedList<EmployeeResponse>> Handle(SearchEmployeesCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchEmployeeSpecs(request);

        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<EmployeeResponse>(items, request!.PageNumber, request!.PageSize, totalCount);
    }
}
