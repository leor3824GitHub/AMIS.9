using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.AcceptanceItems.Update.v1;

public sealed class UpdateAcceptanceItemHandler(
    ILogger<UpdateAcceptanceItemHandler> logger,
    [FromKeyedServices("catalog:acceptanceItems")] IRepository<AcceptanceItem> repository)
    : IRequestHandler<UpdateAcceptanceItemCommand, UpdateAcceptanceItemResponse>
{
    public async Task<UpdateAcceptanceItemResponse> Handle(UpdateAcceptanceItemCommand request, CancellationToken cancellationToken)
    {
        var item = await repository.GetByIdAsync(request.Id, cancellationToken)
                   ?? throw new AcceptanceItemNotFoundException(request.Id);

        item.Update(
            request.AcceptanceId,
            request.PurchaseItemId,
            request.QtyAccepted,
            request.Remarks
        );

        await repository.UpdateAsync(item, cancellationToken);
        logger.LogInformation("AcceptanceItem with id {Id} updated.", request.Id);

        return new UpdateAcceptanceItemResponse(request.Id);
    }
}
