using AMIS.Framework.Core.Identity.Users.Abstractions;
using AMIS.Framework.Core.Persistence;
using AMIS.Shared.Authorization;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.PurchaseRequests.ManageItems.v1;

public sealed class UpdatePurchaseRequestItemHandler(
    ILogger<UpdatePurchaseRequestItemHandler> logger,
    ICurrentUser currentUser,
    [FromKeyedServices("catalog:purchaseRequests")] IRepository<PurchaseRequest> repository)
    : IRequestHandler<UpdatePurchaseRequestItemCommand>
{
    public async Task Handle(UpdatePurchaseRequestItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        var pr = await repository.GetByIdAsync(request.PurchaseRequestId, cancellationToken) ?? throw new InvalidOperationException("PurchaseRequest not found");

        // Authorization check: Only the requester, supply officers, or admins can edit
        var userId = currentUser.GetUserId();
        var isAdmin = currentUser.IsInRole(FshRoles.Admin);
        if (pr.RequestedBy != userId && !isAdmin)
        {
            throw new UnauthorizedAccessException("You are not authorized to edit this purchase request.");
        }

        if (pr.Status != Domain.ValueObjects.PurchaseRequestStatus.Draft)
            throw new InvalidOperationException("Can only update items while in Draft status");

        pr.UpdateItem(request.ItemId, request.ProductId, request.Qty, request.Unit, request.Description);
        await repository.UpdateAsync(pr, cancellationToken);
        logger.LogInformation("Updated item {ItemId} in PurchaseRequest {PRId}", request.ItemId, pr.Id);
    }
}
