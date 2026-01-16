using AMIS.WebApi.Catalog.Domain;
using AMIS.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.Catalog.Application.Issuances.Features.Return.v1;

public sealed class ReturnIssuanceHandler(
    [FromKeyedServices("catalog:issuances")] IRepository<Issuance> repository)
    : IRequestHandler<ReturnIssuanceCommand, ReturnIssuanceResponse>
{
    public async Task<ReturnIssuanceResponse> Handle(ReturnIssuanceCommand request, CancellationToken cancellationToken)
    {
        var issuance = await repository.GetByIdAsync(request.IssuanceId, cancellationToken)
            ?? throw new InvalidOperationException($"Issuance with ID {request.IssuanceId} not found.");

        issuance.MarkAsReturned();

        await repository.UpdateAsync(issuance, cancellationToken);

        return new ReturnIssuanceResponse(issuance.Id);
    }
}
