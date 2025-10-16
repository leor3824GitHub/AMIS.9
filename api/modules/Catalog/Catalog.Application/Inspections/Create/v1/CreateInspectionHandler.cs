using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
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

        foreach (var item in request.Items)
        {
            inspection.AddItem(item.PurchaseItemId, item.QtyInspected, item.QtyPassed, item.QtyFailed, item.Remarks);
        }

        await repository.AddAsync(inspection, cancellationToken);
        logger.LogInformation("Inspection created {InspectionId}", inspection.Id);

        return new CreateInspectionResponse(inspection.Id);
    }
}
