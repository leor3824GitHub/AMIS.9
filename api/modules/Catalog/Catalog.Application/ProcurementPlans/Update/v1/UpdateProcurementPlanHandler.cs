using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.ProcurementPlans.Update.v1;

public sealed class UpdateProcurementPlanHandler(
    ILogger<UpdateProcurementPlanHandler> logger,
    [FromKeyedServices("catalog:procurementPlans")] IRepository<ProcurementPlanHeader> repository)
    : IRequestHandler<UpdateProcurementPlanCommand, UpdateProcurementPlanResponse>
{
    public async Task<UpdateProcurementPlanResponse> Handle(UpdateProcurementPlanCommand request, CancellationToken cancellationToken)
    {
        var plan = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        if (plan is null) throw new InvalidOperationException("Procurement plan not found.");

        plan.UpdateHeader(request.DepartmentId, request.DepartmentName, request.IsSupplemental, request.BudgetType);
        await repository.UpdateAsync(plan, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("Updated procurement plan {ProcurementPlanId}", plan.Id);
        return new UpdateProcurementPlanResponse(plan.Id);
    }
}
