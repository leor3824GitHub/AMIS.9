using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.PurchaseRequests.ManageItems.v1;

public sealed class UpdatePurchaseRequestItemHandler(
    ILogger<UpdatePurchaseRequestItemHandler> logger,
    [FromKeyedServices("catalog:purchaseRequests")] IRepository<PurchaseRequest> repository)
    : IRequestHandler<UpdatePurchaseRequestItemCommand>
{
    public async Task Handle(UpdatePurchaseRequestItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var pr = await repository.GetByIdAsync(request.PurchaseRequestId, cancellationToken) ?? throw new InvalidOperationException("PurchaseRequest not found");
        if (pr.Status != Domain.ValueObjects.PurchaseRequestStatus.Draft)
            throw new InvalidOperationException("Can only update items while in Draft status");

        pr.UpdateItem(request.ItemId, request.ProductId, request.Qty, request.Description, request.Justification);
        await repository.UpdateAsync(pr, cancellationToken);
        logger.LogInformation("Updated item {ItemId} in PurchaseRequest {PRId}", request.ItemId, pr.Id);
    }
}
