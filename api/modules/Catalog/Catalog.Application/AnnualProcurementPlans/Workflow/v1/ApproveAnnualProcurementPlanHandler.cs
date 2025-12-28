using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.AnnualProcurementPlans.Workflow.v1;

public sealed class ApproveAnnualProcurementPlanHandler(
    ILogger<ApproveAnnualProcurementPlanHandler> logger,
    [FromKeyedServices("catalog:annualProcurementPlans")] IRepository<AnnualProcurementPlanHeader> repository)
    : IRequestHandler<ApproveAnnualProcurementPlanCommand>
{
    public async Task Handle(ApproveAnnualProcurementPlanCommand request, CancellationToken cancellationToken)
    {
        var plan = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        if (plan is null) throw new InvalidOperationException("Annual procurement plan not found.");

        plan.Approve(request.ApprovedByUserId);
        await repository.UpdateAsync(plan, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Approved annual procurement plan {AnnualProcurementPlanId}", plan.Id);
    }
}
