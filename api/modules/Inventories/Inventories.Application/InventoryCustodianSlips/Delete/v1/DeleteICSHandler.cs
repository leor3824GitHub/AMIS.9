using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.InventoryCustodianSlips.Delete.v1;

public sealed class DeleteICSHandler(
    ILogger<DeleteICSHandler> logger,
    [FromKeyedServices("inventories:ics")] IRepository<InventoryCustodianSlip> repository)
    : IRequestHandler<DeleteICSCommand, DeleteICSResponse>
{
    public async Task<DeleteICSResponse> Handle(DeleteICSCommand request, CancellationToken cancellationToken)
    {
        var ics = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (ics == null)
        {
            logger.LogWarning("ICS with ID {ICSId} not found", request.Id);
            throw new InvalidOperationException($"ICS with ID {request.Id} not found");
        }

        logger.LogInformation("Deleting ICS {ICSNumber}", ics.ICSNumber);

        await repository.DeleteAsync(ics, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("ICS {ICSNumber} deleted successfully", ics.ICSNumber);

        return new DeleteICSResponse(ics.Id, "ICS deleted successfully");
    }
}
