using AMIS.WebApi.Catalog.Domain;
using AMIS.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.Catalog.Application.Issuances.Features.Cancel.v1;

public sealed class CancelIssuanceHandler(
    [FromKeyedServices("catalog:issuances")] IRepository<Issuance> repository)
    : IRequestHandler<CancelIssuanceCommand, CancelIssuanceResponse>
{
    public async Task<CancelIssuanceResponse> Handle(CancelIssuanceCommand request, CancellationToken cancellationToken)
    {
        var issuance = await repository.GetByIdAsync(request.IssuanceId, cancellationToken)
            ?? throw new InvalidOperationException($"Issuance with ID {request.IssuanceId} not found.");

        issuance.Cancel();

        await repository.UpdateAsync(issuance, cancellationToken);

        return new CancelIssuanceResponse(issuance.Id);
    }
}
