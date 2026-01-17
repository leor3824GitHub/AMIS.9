using AMIS.WebApi.Inventories.Domain;
using AMIS.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.Inventories.Application.Issuances.Features.Reject.v1;

public sealed class RejectIssuanceHandler(
    [FromKeyedServices("inventories:issuances")] IRepository<Issuance> repository)
    : IRequestHandler<RejectIssuanceCommand, RejectIssuanceResponse>
{
    public async Task<RejectIssuanceResponse> Handle(RejectIssuanceCommand request, CancellationToken cancellationToken)
    {
        var issuance = await repository.GetByIdAsync(request.IssuanceId, cancellationToken)
            ?? throw new InvalidOperationException($"Issuance with ID {request.IssuanceId} not found.");

        issuance.Reject(request.RejectionReason);

        await repository.UpdateAsync(issuance, cancellationToken);

        return new RejectIssuanceResponse(
            issuance.Id,
            issuance.RejectionReason!);
    }
}

