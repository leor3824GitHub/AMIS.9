using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Application.AnnualProcurementPlans.Get.v1;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.AnnualProcurementPlans.Items.v1;

public sealed class AddAnnualProcurementPlanItemHandler(
    ILogger<AddAnnualProcurementPlanItemHandler> logger,
    [FromKeyedServices("inventories:annualProcurementPlans")] IRepository<AnnualProcurementPlanHeader> repository)
    : IRequestHandler<AddAnnualProcurementPlanItemCommand, AddAnnualProcurementPlanItemResponse>
{
    public async Task<AddAnnualProcurementPlanItemResponse> Handle(AddAnnualProcurementPlanItemCommand request, CancellationToken cancellationToken)
    {
        var plan = await repository.FirstOrDefaultAsync(new GetAnnualProcurementPlanSpec(request.PlanId), cancellationToken).ConfigureAwait(false);
        if (plan is null) throw new InvalidOperationException("Annual procurement plan not found.");

        var item = plan.AddItem(
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
        logger.LogInformation("Added annual procurement plan item {ItemId} to plan {PlanId}", item.Id, plan.Id);
        return new AddAnnualProcurementPlanItemResponse(item.Id);
    }
}

