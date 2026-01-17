using AMIS.Framework.Core.Exceptions;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Application.ProcurementPlans.Get.v1;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.ProcurementPlans.Workflow.v1;

public sealed class RevertProcurementPlanToDraftHandler(
    ILogger<RevertProcurementPlanToDraftHandler> logger,
    [FromKeyedServices("inventories:procurementPlans")] IRepository<ProcurementPlanHeader> repository)
    : IRequestHandler<RevertProcurementPlanToDraftCommand>
{
    public async Task Handle(RevertProcurementPlanToDraftCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var plan = await repository.FirstOrDefaultAsync(new GetProcurementPlanSpec(request.Id), cancellationToken).ConfigureAwait(false);

        if (plan is null)
        {
            throw new NotFoundException($"Procurement Plan with Id '{request.Id}' was not found.");
        }

        plan.RevertToDraft();

        await repository.UpdateAsync(plan, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("Procurement plan reverted to draft {ProcurementPlanId}", plan.Id);
    }
}

