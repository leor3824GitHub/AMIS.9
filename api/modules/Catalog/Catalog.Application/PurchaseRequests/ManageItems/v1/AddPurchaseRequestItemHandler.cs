using AMIS.Framework.Core.Identity.Users.Abstractions;
using AMIS.Framework.Core.Persistence;
using AMIS.Shared.Authorization;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.PurchaseRequests.ManageItems.v1;

public sealed class AddPurchaseRequestItemHandler(
    ILogger<AddPurchaseRequestItemHandler> logger,
    ICurrentUser currentUser,
    [FromKeyedServices("catalog:purchaseRequests")] IRepository<PurchaseRequest> repository)
    : IRequestHandler<AddPurchaseRequestItemCommand, Guid>
{
    public async Task<Guid> Handle(AddPurchaseRequestItemCommand request, CancellationToken cancellationToken)
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
            throw new InvalidOperationException("Can only add items while in Draft status");

        pr.AddItem(request.ProductId, request.Qty, request.Description, request.Justification);
        await repository.UpdateAsync(pr, cancellationToken);
        var newItem = pr.Items.Last();
        logger.LogInformation("Added item {ItemId} to PurchaseRequest {PRId}", newItem.Id, pr.Id);
        return newItem.Id;
    }
}
