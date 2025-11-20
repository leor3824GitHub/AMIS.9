using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.Acceptances.Cancel.v1;

public sealed class CancelAcceptanceHandler(
    ILogger<CancelAcceptanceHandler> logger,
    [FromKeyedServices("catalog:acceptances")] IRepository<Acceptance> repository)
    : IRequestHandler<CancelAcceptanceCommand, CancelAcceptanceResponse>
{
    public async Task<CancelAcceptanceResponse> Handle(CancelAcceptanceCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var acceptance = await repository.GetByIdAsync(request.Id, cancellationToken) ?? throw new Exception($"Acceptance {request.Id} not found");
        acceptance.Cancel(request.Reason);
        await repository.UpdateAsync(acceptance, cancellationToken);
        logger.LogInformation("Acceptance {AcceptanceId} cancelled.", acceptance.Id);
        return new CancelAcceptanceResponse(acceptance.Id);
    }
}
