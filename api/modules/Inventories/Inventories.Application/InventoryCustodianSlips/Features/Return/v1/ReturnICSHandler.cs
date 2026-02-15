using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.InventoryCustodianSlips.Features.Return.v1;

public sealed class ReturnICSHandler(
    ILogger<ReturnICSHandler> logger,
    [FromKeyedServices("inventories:ics")] IRepository<InventoryCustodianSlip> repository)
    : IRequestHandler<ReturnICSCommand, ReturnICSResponse>
{
    public async Task<ReturnICSResponse> Handle(ReturnICSCommand request, CancellationToken cancellationToken)
    {
        var ics = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (ics == null)
        {
            logger.LogWarning("ICS with ID {ICSId} not found", request.Id);
            throw new InvalidOperationException($"ICS with ID {request.Id} not found");
        }

        logger.LogInformation("Returning ICS {ICSNumber}", ics.ICSNumber);

        ics.Return(request.ReturnDate, request.ReceivedByEmployeeId, request.ReturnRemarks);

        await repository.UpdateAsync(ics, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("ICS {ICSNumber} returned successfully", ics.ICSNumber);

        return new ReturnICSResponse(ics.Id, "ICS returned successfully");
    }
}
