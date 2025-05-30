using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.InspectionItems.Create.v1;

public sealed class CreateInspectionItemHandler(
    ILogger<CreateInspectionItemHandler> logger,
    [FromKeyedServices("catalog:acceptanceItems")] IRepository<InspectionItem> repository)
    : IRequestHandler<CreateInspectionItemCommand, CreateInspectionItemResponse>
{
    public async Task<CreateInspectionItemResponse> Handle(CreateInspectionItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var entity = InspectionItem.Create(
            request.InspectionId,
            request.PurchaseItemId,
            request.QtyInspected,
            request.QtyPassed,
            request.QtyFailed,
            request.Remarks);

        await repository.AddAsync(entity, cancellationToken);
        logger.LogInformation("Created inspection item {InspectionItemId}", entity.Id);

        return new CreateInspectionItemResponse(entity.Id);
    }
}
