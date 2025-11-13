using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Inspections.ManageItems.v1;

public sealed class RemoveInspectionItemHandler(
    [FromKeyedServices("catalog:inspections")] IRepository<Inspection> repository)
    : IRequestHandler<RemoveInspectionItemCommand>
{
    public async Task Handle(RemoveInspectionItemCommand request, CancellationToken cancellationToken)
    {
        var inspection = await repository.GetByIdAsync(request.InspectionId, cancellationToken);
        if (inspection is null)
            throw new Exception($"Inspection {request.InspectionId} not found");

        var item = inspection.Items.FirstOrDefault(i => i.Id == request.ItemId);
        if (item is not null)
        {
            inspection.RemoveItem(item);
            await repository.UpdateAsync(inspection, cancellationToken);
        }
    }
}
