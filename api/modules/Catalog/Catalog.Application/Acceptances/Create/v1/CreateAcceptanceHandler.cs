using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Acceptances.Create.v1;

public sealed class CreateAcceptanceHandler(
    ILogger<CreateAcceptanceHandler> logger,
    [FromKeyedServices("catalog:acceptances")] IRepository<Acceptance> repository)
    : IRequestHandler<CreateAcceptanceCommand, CreateAcceptanceResponse>
{
    public async Task<CreateAcceptanceResponse> Handle(CreateAcceptanceCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var acceptance = Acceptance.Create(
            purchaseId: request.PurchaseId,
            supplyOfficerId: request.SupplyOfficerId,
            acceptanceDate: request.AcceptanceDate,
            remarks: request.Remarks
        );

        if (request.Items is not null)
        {
            foreach (var item in request.Items)
            {
                acceptance.AddItem(item.PurchaseItemId, item.QtyAccepted, item.Remarks);
            }
        }

        await repository.AddAsync(acceptance, cancellationToken);
        logger.LogInformation("Acceptance created {AcceptanceId}", acceptance.Id);

        return new CreateAcceptanceResponse(acceptance.Id);
    }
}
