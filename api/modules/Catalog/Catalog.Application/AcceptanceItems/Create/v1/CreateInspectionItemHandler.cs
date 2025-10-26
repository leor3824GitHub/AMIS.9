using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AMIS.WebApi.Catalog.Application.AcceptanceItems.Specifications;

namespace AMIS.WebApi.Catalog.Application.AcceptanceItems.Create.v1;

public sealed class CreateAcceptanceItemHandler(
    ILogger<CreateAcceptanceItemHandler> logger,
    [FromKeyedServices("catalog:acceptanceItems")] IRepository<AcceptanceItem> repository)
    : IRequestHandler<CreateAcceptanceItemCommand, CreateAcceptanceItemResponse>
{
    public async Task<CreateAcceptanceItemResponse> Handle(CreateAcceptanceItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Single-shot guard: prevent creating another acceptance for the same purchase item
        var existsSpec = new AcceptanceItemByPurchaseItemIdSpec(request.PurchaseItemId);
        var existing = await repository.FirstOrDefaultAsync(existsSpec, cancellationToken);
        if (existing is not null)
        {
            throw new InvalidOperationException("An acceptance has already been recorded for this purchase item. Single-shot acceptance is enforced.");
        }

        var entity = AcceptanceItem.Create(
            request.AcceptanceId,
            request.PurchaseItemId,
            request.QtyAccepted,
            request.Remarks);

        await repository.AddAsync(entity, cancellationToken);
        logger.LogInformation("Created acceptance item {AcceptanceItemId}", entity.Id);

        return new CreateAcceptanceItemResponse(entity.Id);
    }
}
