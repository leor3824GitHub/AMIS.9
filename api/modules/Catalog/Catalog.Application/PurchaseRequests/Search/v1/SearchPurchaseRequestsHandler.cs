using AMIS.Framework.Core.Identity.Users.Abstractions;
using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Persistence;
using AMIS.Shared.Authorization;
using AMIS.WebApi.Catalog.Application.PurchaseRequests.Get.v1;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Application.Employees.Search.v1;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.PurchaseRequests.Search.v1;

public sealed class SearchPurchaseRequestsHandler(
    ILogger<SearchPurchaseRequestsHandler> logger,
    ICurrentUser currentUser,
    [FromKeyedServices("catalog:purchaseRequests")] IReadRepository<PurchaseRequest> repository,
    [FromKeyedServices("catalog:employees")] IReadRepository<Employee> employeeRepository)
    : IRequestHandler<SearchPurchaseRequestsCommand, AMIS.Framework.Core.Paging.PagedList<PurchaseRequestResponse>>
{
    public async Task<AMIS.Framework.Core.Paging.PagedList<PurchaseRequestResponse>> Handle(SearchPurchaseRequestsCommand request, CancellationToken cancellationToken)
    {
        // Apply role-based filtering
        var userId = currentUser.GetUserId();
        var isSupplyOfficer = currentUser.IsInRole(FshRoles.SupplyOfficer);
        var isAdmin = currentUser.IsInRole(FshRoles.Admin);

        // If not supply officer or admin, filter to only show user's own requests
        if (!isSupplyOfficer && !isAdmin && !request.RequestedBy.HasValue)
        {
            // Map current user to their Employee and filter by Employee.Id
            var employee = await employeeRepository.FirstOrDefaultAsync(new EmployeeByUserIdSpec(userId), cancellationToken);

            // If no employee found, force a non-match to return empty set
            request.RequestedBy = employee?.Id ?? Guid.Empty;
        }

        var specs = new SearchPurchaseRequestSpecs(request);
        var filter = new PaginationFilter { PageNumber = request.PageNumber, PageSize = request.PageSize, OrderBy = request.OrderBy, Keyword = request.Keyword };
        var results = await repository.PaginatedListAsync(specs, filter, cancellationToken);
        logger.LogInformation("Searched PurchaseRequests - page {PageNumber}, User: {UserId}, IsSupplyOfficer: {IsSupplyOfficer}",
            request.PageNumber, userId, isSupplyOfficer);
        return results;
    }
}
