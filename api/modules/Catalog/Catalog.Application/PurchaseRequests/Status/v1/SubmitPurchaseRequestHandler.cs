using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.PurchaseRequests.Status.v1;

public sealed class SubmitPurchaseRequestHandler(
    ILogger<SubmitPurchaseRequestHandler> logger,
    [FromKeyedServices("catalog:purchaseRequests")] IRepository<PurchaseRequest> repository)
    : IRequestHandler<SubmitPurchaseRequestCommand>
{
    public async Task Handle(SubmitPurchaseRequestCommand request, CancellationToken cancellationToken)
    {
        var pr = await repository.GetByIdAsync(request.PurchaseRequestId, cancellationToken) ?? throw new InvalidOperationException("PurchaseRequest not found");
        pr.Submit();
        await repository.UpdateAsync(pr, cancellationToken);
        logger.LogInformation("Submitted PurchaseRequest {PRId}", pr.Id);
    }
}
