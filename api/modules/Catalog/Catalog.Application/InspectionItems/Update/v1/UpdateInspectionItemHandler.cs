using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.Inspections.UpdateItem.v1;

public sealed class UpdateInspectionItemHandler(
    ILogger<UpdateInspectionItemHandler> logger,
    [FromKeyedServices("catalog:inspectionitems")] IRepository<InspectionItem> repository)
    : IRequestHandler<UpdateInspectionItemCommand, Guid>
{
    public async Task<Guid> Handle(UpdateInspectionItemCommand request, CancellationToken cancellationToken)
    {
        var item = await repository.GetByIdAsync(request.Id, cancellationToken)
                   ?? throw new InspectionItemNotFoundException(request.Id);

        item.Update(
            request.InspectionId,
            request.PurchaseItemId,
            request.QtyInspected,
            request.QtyPassed,
            request.QtyFailed,
            request.Remarks
        );

        await repository.UpdateAsync(item, cancellationToken);
        logger.LogInformation("InspectionItem with id {Id} updated.", request.Id);

        return request.Id;
    }
}
