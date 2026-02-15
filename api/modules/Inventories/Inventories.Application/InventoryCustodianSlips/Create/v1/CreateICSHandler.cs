using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.InventoryCustodianSlips.Create.v1;

public sealed class CreateICSHandler(
    ILogger<CreateICSHandler> logger,
    [FromKeyedServices("inventories:ics")] IRepository<InventoryCustodianSlip> repository)
    : IRequestHandler<CreateICSCommand, CreateICSResponse>
{
    public async Task<CreateICSResponse> Handle(
        CreateICSCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation(
                "Creating new ICS with number: {ICSNumber} for employee: {EmployeeId}",
                request.ICSNumber,
                request.EmployeeId);

            var ics = new InventoryCustodianSlip(
                request.ICSNumber,
                request.EmployeeId,
                request.IssuanceDate,
                request.IssuancePurpose,
                request.IssuanceLocation,
                request.Notes);

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

            await repository.AddAsync(ics, cancellationToken);
            await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            logger.LogInformation(
                "ICS {ICSNumber} created as Draft with {Count} line items.",
                request.ICSNumber,
                request.LineItems.Count);

            return new CreateICSResponse(
                ics.Id,
                ics.ICSNumber,
                ics.Created.DateTime);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating ICS {ICSNumber}", request.ICSNumber);
            throw;
        }
    }
}
