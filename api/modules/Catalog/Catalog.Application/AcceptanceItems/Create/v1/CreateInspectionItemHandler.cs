using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.AcceptanceItems.Create.v1;

public sealed class CreateAcceptanceItemHandler(
    ILogger<CreateAcceptanceItemHandler> logger,
    [FromKeyedServices("catalog:acceptanceItems")] IRepository<AcceptanceItem> repository)
    : IRequestHandler<CreateAcceptanceItemCommand, CreateAcceptanceItemResponse>
{
    public async Task<CreateAcceptanceItemResponse> Handle(CreateAcceptanceItemCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var entity = AcceptanceItem.Create(
            request.AcceptanceId,
            request.PurchaseItemId,
            request.QtyAccepted,
            request.Remarks);

        await repository.AddAsync(entity, cancellationToken);
        logger.LogInformation("Created inspection item {AcceptanceItemId}", entity.Id);

        return new CreateAcceptanceItemResponse(entity.Id);
    }
}
