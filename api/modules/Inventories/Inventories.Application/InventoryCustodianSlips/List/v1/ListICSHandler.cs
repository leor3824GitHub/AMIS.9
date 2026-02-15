using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.InventoryCustodianSlips.List.v1;

public sealed class ListICSHandler(
    ILogger<ListICSHandler> logger,
    [FromKeyedServices("inventories:ics")] IReadRepository<InventoryCustodianSlip> repository)
    : IRequestHandler<ListICSQuery, ListICSResponse>
{
    public async Task<ListICSResponse> Handle(ListICSQuery request, CancellationToken cancellationToken)
    {
        var allIcs = await repository.ListAsync(cancellationToken);
        var total = allIcs.Count();

        var items = allIcs
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new ListICSItemResponse(
                x.Id,
                x.ICSNumber,
                x.EmployeeId,
                x.IssuanceDate,
                x.Status.ToString(),
                x.GetLineItemsCount(),
                x.GetTotalAcquisitionCost()))
            .ToList();

        return new ListICSResponse(items, total, request.PageNumber, request.PageSize);
    }
}
