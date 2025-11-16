using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.PurchaseRequests.Status.v1;

public sealed class CancelPurchaseRequestHandler(
    ILogger<CancelPurchaseRequestHandler> logger,
    [FromKeyedServices("catalog:purchaseRequests")] IRepository<PurchaseRequest> repository)
    : IRequestHandler<CancelPurchaseRequestCommand>
{
    public async Task Handle(CancelPurchaseRequestCommand request, CancellationToken cancellationToken)
    {
        var pr = await repository.GetByIdAsync(request.PurchaseRequestId, cancellationToken) ?? throw new InvalidOperationException("PurchaseRequest not found");
        pr.Cancel(request.Reason);
        await repository.UpdateAsync(pr, cancellationToken);
        logger.LogInformation("Cancelled PurchaseRequest {PRId}", pr.Id);
    }
}
