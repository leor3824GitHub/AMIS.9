using AMIS.Framework.Core.Exceptions;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Application.AnnualProcurementPlans.Get.v1;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.AnnualProcurementPlans.Workflow.v1;

public sealed class RevertAnnualProcurementPlanToDraftHandler(
    ILogger<RevertAnnualProcurementPlanToDraftHandler> logger,
    [FromKeyedServices("inventories:annualProcurementPlans")] IRepository<AnnualProcurementPlanHeader> repository)
    : IRequestHandler<RevertAnnualProcurementPlanToDraftCommand>
{
    public async Task Handle(RevertAnnualProcurementPlanToDraftCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var plan = await repository.FirstOrDefaultAsync(new GetAnnualProcurementPlanSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (plan is null)
        {
            throw new NotFoundException($"Annual Procurement Plan with Id '{request.Id}' was not found.");
        }

        plan.RevertToDraft();

        await repository.UpdateAsync(plan, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Annual procurement plan reverted to draft {AnnualProcurementPlanId}", plan.Id);
    }
}

