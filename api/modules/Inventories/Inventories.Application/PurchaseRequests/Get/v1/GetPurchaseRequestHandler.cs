using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.PurchaseRequests.Get.v1;

public sealed class GetPurchaseRequestHandler : IRequestHandler<GetPurchaseRequestCommand, PurchaseRequestResponse?>
{
    private readonly ILogger<GetPurchaseRequestHandler> _logger;
    private readonly IReadRepository<PurchaseRequest> _repository;

    public GetPurchaseRequestHandler(ILogger<GetPurchaseRequestHandler> logger,
        [FromKeyedServices("inventories:purchaseRequests")] IReadRepository<PurchaseRequest> repository)
    {
        _logger = logger;
        _repository = repository;
    }

    public async Task<PurchaseRequestResponse?> Handle(GetPurchaseRequestCommand request, CancellationToken cancellationToken)
    {
        var pr = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (pr is null) return null;
        var response = new PurchaseRequestResponse(
            pr.Id,
            pr.RequestDate,
            pr.RequestedBy,
            pr.Purpose,
            pr.Status,
            pr.ApprovalRemarks,
            pr.ApprovedBy,
            pr.ApprovedOn,
            pr.Items.Select(i => new PurchaseRequestItemResponse(i.Id, i.ProductId, i.ManualProductName, i.Qty, i.Unit, i.Description, null)).ToList()
        );
        _logger.LogInformation("Fetched PurchaseRequest {PRId}", pr.Id);
        return response;
    }
}

