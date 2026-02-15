using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.InventoryCustodianSlips.Update.v1;

public sealed class UpdateICSHandler(
    ILogger<UpdateICSHandler> logger,
    [FromKeyedServices("inventories:ics")] IRepository<InventoryCustodianSlip> repository)
    : IRequestHandler<UpdateICSCommand, UpdateICSResponse>
{
    public async Task<UpdateICSResponse> Handle(UpdateICSCommand request, CancellationToken cancellationToken)
    {
        var ics = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (ics == null)
        {
            logger.LogWarning("ICS with ID {ICSId} not found", request.Id);
            throw new InvalidOperationException($"ICS with ID {request.Id} not found");
        }

        logger.LogInformation("Updating ICS {ICSNumber}", ics.ICSNumber);

        ics.UpdateHeader(
            request.EmployeeId,
            request.IssuanceDate,
            request.IssuancePurpose,
            request.IssuanceLocation,
            request.Notes);

        ics.ClearLineItems();

        var lineItems = request.LineItems
            .Select(x => new ICSLineItem(
                x.PropertyCode,
                x.Description,
                x.Quantity,
                x.DateAcquired,
                x.UnitCost,
                x.Condition,
                x.Remarks))
            .ToList();

        ics.AddLineItems(lineItems);

        await repository.UpdateAsync(ics, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("ICS {ICSNumber} updated successfully", ics.ICSNumber);

        return new UpdateICSResponse(
            ics.Id,
            ics.ICSNumber,
            ics.LastModified.DateTime);
    }
}
