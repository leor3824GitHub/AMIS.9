using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.InventoryCustodianSlips.Get.v1;

public sealed class GetICSHandler(
    ILogger<GetICSHandler> logger,
    [FromKeyedServices("inventories:ics")] IReadRepository<InventoryCustodianSlip> repository)
    : IRequestHandler<GetICSQuery, GetICSResponse>
{
    public async Task<GetICSResponse> Handle(GetICSQuery request, CancellationToken cancellationToken)
    {
        var ics = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (ics == null)
        {
            logger.LogWarning("ICS with ID {ICSId} not found", request.Id);
            throw new InvalidOperationException($"ICS with ID {request.Id} not found");
        }

        var lineItems = ics.LineItems
            .Select(x => new ICSLineItemResponse(
                x.PropertyCode,
                x.Description,
                x.Quantity,
                x.DateAcquired,
                x.UnitCost,
                x.AcquisitionCost,
                x.Condition,
                x.Remarks))
            .ToList();

        return new GetICSResponse(
            ics.Id,
            ics.ICSNumber,
            ics.EmployeeId,
            ics.IssuanceDate,
            ics.IssuancePurpose,
            ics.IssuanceLocation,
            ics.Notes,
            ics.Status.ToString(),
            lineItems,
            ics.GetTotalAcquisitionCost());
    }
}
