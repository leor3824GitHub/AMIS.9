using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.PurchaseRequests.Status.v1;

public sealed class ApprovePurchaseRequestHandler(
    ILogger<ApprovePurchaseRequestHandler> logger,
    [FromKeyedServices("catalog:purchaseRequests")] IRepository<PurchaseRequest> repository)
    : IRequestHandler<ApprovePurchaseRequestCommand>
{
    public async Task Handle(ApprovePurchaseRequestCommand request, CancellationToken cancellationToken)
    {
        var pr = await repository.GetByIdAsync(request.PurchaseRequestId, cancellationToken) ?? throw new InvalidOperationException("PurchaseRequest not found");
        pr.Approve(request.ApprovedBy, request.Remarks);
        await repository.UpdateAsync(pr, cancellationToken);
        logger.LogInformation("Approved PurchaseRequest {PRId}", pr.Id);
    }
}
