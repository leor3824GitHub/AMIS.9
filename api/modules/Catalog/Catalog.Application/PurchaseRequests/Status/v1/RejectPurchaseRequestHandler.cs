using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.PurchaseRequests.Status.v1;

public sealed class RejectPurchaseRequestHandler(
    ILogger<RejectPurchaseRequestHandler> logger,
    [FromKeyedServices("catalog:purchaseRequests")] IRepository<PurchaseRequest> repository)
    : IRequestHandler<RejectPurchaseRequestCommand>
{
    public async Task Handle(RejectPurchaseRequestCommand request, CancellationToken cancellationToken)
    {
        var pr = await repository.GetByIdAsync(request.PurchaseRequestId, cancellationToken) ?? throw new InvalidOperationException("PurchaseRequest not found");
        pr.Reject(request.RejectedBy, request.Reason);
        await repository.UpdateAsync(pr, cancellationToken);
        logger.LogInformation("Rejected PurchaseRequest {PRId}", pr.Id);
    }
}
