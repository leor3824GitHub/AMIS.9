using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.PurchaseRequests.Create.v1;

public sealed class CreatePurchaseRequestHandler(
    ILogger<CreatePurchaseRequestHandler> logger,
    [FromKeyedServices("catalog:purchaseRequests")] IRepository<PurchaseRequest> repository)
    : IRequestHandler<CreatePurchaseRequestCommand, CreatePurchaseRequestResponse>
{
    public async Task<CreatePurchaseRequestResponse> Handle(CreatePurchaseRequestCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var pr = PurchaseRequest.Create(request.RequestedBy, request.Purpose, request.RequestDate);

        if (request.Items is not null && request.Items.Count > 0)
        {
            foreach (var item in request.Items)
            {
                pr.AddItem(item.ProductId, item.Qty, item.Unit, item.Description);
            }
        }

        await repository.AddAsync(pr, cancellationToken);
        logger.LogInformation("PurchaseRequest created {PurchaseRequestId}", pr.Id);
        return new CreatePurchaseRequestResponse(pr.Id);
    }
}
