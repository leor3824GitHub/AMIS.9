using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.PurchaseRequests.ManageItems.v1;

public sealed class DeletePurchaseRequestItemHandler(
    ILogger<DeletePurchaseRequestItemHandler> logger,
    [FromKeyedServices("catalog:purchaseRequests")] IRepository<PurchaseRequest> repository)
    : IRequestHandler<DeletePurchaseRequestItemCommand>
{
    public async Task Handle(DeletePurchaseRequestItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var pr = await repository.GetByIdAsync(request.PurchaseRequestId, cancellationToken) ?? throw new InvalidOperationException("PurchaseRequest not found");
        if (pr.Status != Domain.ValueObjects.PurchaseRequestStatus.Draft)
            throw new InvalidOperationException("Can only delete items while in Draft status");

        pr.RemoveItem(request.ItemId);
        await repository.UpdateAsync(pr, cancellationToken);
        logger.LogInformation("Removed item {ItemId} from PurchaseRequest {PRId}", request.ItemId, pr.Id);
    }
}
