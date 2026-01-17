using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Application.AnnualProcurementPlans.Get.v1;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.AnnualProcurementPlans.Items.v1;

public sealed class UpdateAnnualProcurementPlanItemHandler(
    ILogger<UpdateAnnualProcurementPlanItemHandler> logger,
    [FromKeyedServices("inventories:annualProcurementPlans")] IRepository<AnnualProcurementPlanHeader> repository)
    : IRequestHandler<UpdateAnnualProcurementPlanItemCommand>
{
    public async Task Handle(UpdateAnnualProcurementPlanItemCommand request, CancellationToken cancellationToken)
    {
        var plan = await repository.FirstOrDefaultAsync(new GetAnnualProcurementPlanSpec(request.PlanId), cancellationToken).ConfigureAwait(false);
        if (plan is null) throw new InvalidOperationException("Annual procurement plan not found.");

        plan.UpdateItem(
            request.ItemId,
            request.DepartmentId,
            request.DepartmentName,
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
        logger.LogInformation("Updated annual procurement plan item {ItemId} on plan {PlanId}", request.ItemId, request.PlanId);
    }
}

