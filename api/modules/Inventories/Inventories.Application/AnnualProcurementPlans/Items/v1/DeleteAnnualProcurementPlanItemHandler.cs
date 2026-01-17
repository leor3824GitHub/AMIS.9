using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Application.AnnualProcurementPlans.Get.v1;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.AnnualProcurementPlans.Items.v1;

public sealed class DeleteAnnualProcurementPlanItemHandler(
    ILogger<DeleteAnnualProcurementPlanItemHandler> logger,
    [FromKeyedServices("inventories:annualProcurementPlans")] IRepository<AnnualProcurementPlanHeader> repository)
    : IRequestHandler<DeleteAnnualProcurementPlanItemCommand>
{
    public async Task Handle(DeleteAnnualProcurementPlanItemCommand request, CancellationToken cancellationToken)
    {
        var plan = await repository.FirstOrDefaultAsync(new GetAnnualProcurementPlanSpec(request.PlanId), cancellationToken).ConfigureAwait(false);
        if (plan is null) throw new InvalidOperationException("Annual procurement plan not found.");

        plan.DeleteItem(request.ItemId);

        await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Deleted annual procurement plan item {ItemId} from plan {PlanId}", request.ItemId, request.PlanId);
    }
}

