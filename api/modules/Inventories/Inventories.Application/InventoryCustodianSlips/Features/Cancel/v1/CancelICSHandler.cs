using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.InventoryCustodianSlips.Features.Cancel.v1;

public sealed class CancelICSHandler(
    ILogger<CancelICSHandler> logger,
    [FromKeyedServices("inventories:ics")] IRepository<InventoryCustodianSlip> repository)
    : IRequestHandler<CancelICSCommand, CancelICSResponse>
{
    public async Task<CancelICSResponse> Handle(CancelICSCommand request, CancellationToken cancellationToken)
    {
        var ics = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (ics == null)
        {
            logger.LogWarning("ICS with ID {ICSId} not found", request.Id);
            throw new InvalidOperationException($"ICS with ID {request.Id} not found");
        }

        logger.LogInformation("Cancelling ICS {ICSNumber}", ics.ICSNumber);

        ics.Cancel();

        await repository.UpdateAsync(ics, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("ICS {ICSNumber} cancelled successfully", ics.ICSNumber);

        return new CancelICSResponse(ics.Id, "ICS cancelled successfully");
    }
}
