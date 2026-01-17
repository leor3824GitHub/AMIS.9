using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.ValueObjects;
using AMIS.Framework.Core.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.Catalog.Application.Issuances.Features.Accept.v1;

public sealed class AcceptIssuanceHandler(
    [FromKeyedServices("inventories:issuances")] IRepository<Issuance> repository)
    : IRequestHandler<AcceptIssuanceCommand, AcceptIssuanceResponse>
{
    public async Task<AcceptIssuanceResponse> Handle(AcceptIssuanceCommand request, CancellationToken cancellationToken)
    {
        var issuance = await repository.GetByIdAsync(request.IssuanceId, cancellationToken)
            ?? throw new InvalidOperationException($"Issuance with ID {request.IssuanceId} not found.");

        var signature = DigitalSignature.Create(
            signatureData: $"ISSUANCE_ACCEPT_{request.IssuanceId}_{request.SignedByEmployeeId}",
            employeeId: request.SignedByEmployeeId);

        issuance.Accept(signature);

        await repository.UpdateAsync(issuance, cancellationToken);

        return new AcceptIssuanceResponse(
            issuance.Id,
            issuance.AcceptedOn!.Value,
            issuance.CustodianId!.Value);
    }
}

