using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Application.ProcurementPlans.Get.v1;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.ProcurementPlans.Items.v1;

public sealed class AddProcurementPlanItemHandler(
    ILogger<AddProcurementPlanItemHandler> logger,
    [FromKeyedServices("catalog:procurementPlans")] IRepository<ProcurementPlanHeader> repository)
    : IRequestHandler<AddProcurementPlanItemCommand, AddProcurementPlanItemResponse>
{
    public async Task<AddProcurementPlanItemResponse> Handle(AddProcurementPlanItemCommand request, CancellationToken cancellationToken)
    {
        var plan = await repository.FirstOrDefaultAsync(new GetProcurementPlanSpec(request.PlanId), cancellationToken).ConfigureAwait(false);
        if (plan is null) throw new InvalidOperationException("Procurement plan not found.");

        var item = plan.AddItem(
            request.PapCode,
            request.Description,
            request.ProjectType,
            request.Quantity,
            request.UnitOfMeasure,
            request.UnitCost,
            request.Mode,
            request.IsEarlyProcurement,
            request.ScheduleMonth,
            request.FundingSource,
            request.Remarks);

        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Added procurement plan item {ItemId} to plan {PlanId}", item.Id, plan.Id);
        return new AddProcurementPlanItemResponse(item.Id);
    }
}
