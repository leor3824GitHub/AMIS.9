using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.Inspections.Create.v1;

public sealed class CreateInspectionHandler(
    ILogger<CreateInspectionHandler> logger,
    [FromKeyedServices("catalog:inspections")] IRepository<Inspection> repository)
    : IRequestHandler<CreateInspectionCommand, CreateInspectionResponse>
{
    public async Task<CreateInspectionResponse> Handle(CreateInspectionCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var inspection = Inspection.Create(
            purchaseId: request.PurchaseId,
            employeeId: request.InspectorId,
            inspectedOn: request.InspectionDate,
            approved: false,
            remarks: request.Remarks
        );

        if (request.Items is not null)
        {
            foreach (var item in request.Items)
            {
                var status = item.InspectionItemStatus ?? InspectionItemStatus.NotInspected;
                var entity = inspection.AddItem(item.PurchaseItemId, item.QtyInspected, item.QtyPassed, item.QtyFailed, item.Remarks, status);
            }
        }

        await repository.AddAsync(inspection, cancellationToken);
        logger.LogInformation("Inspection created {InspectionId}", inspection.Id);

        return new CreateInspectionResponse(inspection.Id);
    }
}
