using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Inspections.ManageItems.v1;

public sealed class AddInspectionItemHandler(
    [FromKeyedServices("catalog:inspections")] IRepository<Inspection> repository)
    : IRequestHandler<AddInspectionItemCommand, AddInspectionItemResponse>
{
    public async Task<AddInspectionItemResponse> Handle(AddInspectionItemCommand request, CancellationToken cancellationToken)
    {
        var inspection = await repository.GetByIdAsync(request.InspectionId, cancellationToken);
        if (inspection is null)
            throw new Exception($"Inspection {request.InspectionId} not found");

        var item = inspection.AddItem(request.PurchaseItemId, request.QtyInspected, request.QtyPassed, request.QtyFailed, request.Remarks, request.InspectionItemStatus);
        await repository.UpdateAsync(inspection, cancellationToken);
        return new AddInspectionItemResponse(item.Id);
    }
}
