using AMIS.Framework.Core.Exceptions;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Application.AnnualProcurementPlans.Get.v1;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.AnnualProcurementPlans.Workflow.v1;

public sealed class RejectAnnualProcurementPlanHandler(
    ILogger<RejectAnnualProcurementPlanHandler> logger,
    [FromKeyedServices("catalog:annualProcurementPlans")] IRepository<AnnualProcurementPlanHeader> repository)
    : IRequestHandler<RejectAnnualProcurementPlanCommand>
{
    public async Task Handle(RejectAnnualProcurementPlanCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var plan = await repository.FirstOrDefaultAsync(new GetAnnualProcurementPlanSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (plan is null)
        {
            throw new NotFoundException($"Annual Procurement Plan with Id '{request.Id}' was not found.");
        }

        plan.Reject(request.RejectedByUserId, request.Reason);

        await repository.UpdateAsync(plan, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Annual procurement plan rejected {AnnualProcurementPlanId}", plan.Id);
    }
}
