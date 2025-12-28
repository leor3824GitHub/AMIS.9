using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.ProcurementPlans.Items.v1;

public sealed class DeleteProcurementPlanItemHandler(
    ILogger<DeleteProcurementPlanItemHandler> logger,
    [FromKeyedServices("catalog:procurementPlans")] IRepository<ProcurementPlanHeader> repository)
    : IRequestHandler<DeleteProcurementPlanItemCommand>
{
    public async Task Handle(DeleteProcurementPlanItemCommand request, CancellationToken cancellationToken)
    {
        var plan = await repository.GetByIdAsync(request.PlanId, cancellationToken).ConfigureAwait(false);
        if (plan is null) throw new InvalidOperationException("Procurement plan not found.");

        plan.DeleteItem(request.ItemId);
        await repository.UpdateAsync(plan, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Deleted procurement plan item {ItemId} from plan {PlanId}", request.ItemId, request.PlanId);
    }
}
