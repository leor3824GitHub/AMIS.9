using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Inspections.ManageItems.v1;

public sealed class UpdateInspectionItemHandler(
    [FromKeyedServices("catalog:inspections")] IRepository<Inspection> repository)
    : IRequestHandler<UpdateInspectionItemCommand>
{
    public async Task Handle(UpdateInspectionItemCommand request, CancellationToken cancellationToken)
    {
        var inspection = await repository.GetByIdAsync(request.InspectionId, cancellationToken);
        if (inspection is null)
            throw new Exception($"Inspection {request.InspectionId} not found");

        inspection.UpdateItem(request.ItemId, request.QtyInspected, request.QtyPassed, request.QtyFailed, request.Remarks, request.InspectionItemStatus);
        await repository.UpdateAsync(inspection, cancellationToken);
    }
}
