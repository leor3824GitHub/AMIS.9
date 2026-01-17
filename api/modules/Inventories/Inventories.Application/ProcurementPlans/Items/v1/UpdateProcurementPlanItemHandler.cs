using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Application.ProcurementPlans.Get.v1;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.ProcurementPlans.Items.v1;

public sealed class UpdateProcurementPlanItemHandler(
    ILogger<UpdateProcurementPlanItemHandler> logger,
    [FromKeyedServices("inventories:procurementPlans")] IRepository<ProcurementPlanHeader> repository)
    : IRequestHandler<UpdateProcurementPlanItemCommand>
{
    public async Task Handle(UpdateProcurementPlanItemCommand request, CancellationToken cancellationToken)
    {
        var plan = await repository.FirstOrDefaultAsync(new GetProcurementPlanSpec(request.PlanId), cancellationToken).ConfigureAwait(false);
        if (plan is null) throw new InvalidOperationException("Procurement plan not found.");

        plan.UpdateItem(
            request.ItemId,
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
        logger.LogInformation("Updated procurement plan item {ItemId} on plan {PlanId}", request.ItemId, request.PlanId);
    }
}

