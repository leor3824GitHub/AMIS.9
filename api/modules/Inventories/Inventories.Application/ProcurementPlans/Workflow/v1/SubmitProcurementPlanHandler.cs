using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.ProcurementPlans.Workflow.v1;

public sealed class SubmitProcurementPlanHandler(
    ILogger<SubmitProcurementPlanHandler> logger,
    [FromKeyedServices("inventories:procurementPlans")] IRepository<ProcurementPlanHeader> repository)
    : IRequestHandler<SubmitProcurementPlanCommand>
{
    public async Task Handle(SubmitProcurementPlanCommand request, CancellationToken cancellationToken)
    {
        var plan = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        if (plan is null) throw new InvalidOperationException("Procurement plan not found.");

        plan.Submit();
        await repository.UpdateAsync(plan, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Submitted procurement plan {ProcurementPlanId}", plan.Id);
    }
}

