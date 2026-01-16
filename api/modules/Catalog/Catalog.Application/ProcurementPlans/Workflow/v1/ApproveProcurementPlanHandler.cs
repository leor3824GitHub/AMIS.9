using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.ProcurementPlans.Workflow.v1;

public sealed class ApproveProcurementPlanHandler(
    ILogger<ApproveProcurementPlanHandler> logger,
    [FromKeyedServices("catalog:procurementPlans")] IRepository<ProcurementPlanHeader> repository)
    : IRequestHandler<ApproveProcurementPlanCommand>
{
    public async Task Handle(ApproveProcurementPlanCommand request, CancellationToken cancellationToken)
    {
        var plan = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        if (plan is null) throw new InvalidOperationException("Procurement plan not found.");

        plan.Approve(request.ApprovedByUserId);
        await repository.UpdateAsync(plan, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Approved procurement plan {ProcurementPlanId}", plan.Id);
    }
}
