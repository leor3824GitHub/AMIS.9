using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Acceptances.Update.v1;

public sealed class UpdateAcceptanceHandler(
    ILogger<UpdateAcceptanceHandler> logger,
    [FromKeyedServices("catalog:acceptances")] IRepository<Acceptance> repository)
    : IRequestHandler<UpdateAcceptanceCommand, UpdateAcceptanceResponse>
{
    public async Task<UpdateAcceptanceResponse> Handle(UpdateAcceptanceCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var inspection = await repository.GetByIdAsync(request.Id, cancellationToken);
        _ = inspection ?? throw new AcceptanceNotFoundException(request.Id);

        inspection.Update(request.SupplyOfficerId, request.AcceptanceDate, request.Remarks);

        await repository.UpdateAsync(inspection, cancellationToken);
        logger.LogInformation("Acceptance {AcceptanceId} updated (without item changes).", inspection.Id);

        return new UpdateAcceptanceResponse(inspection.Id);
    }
}
