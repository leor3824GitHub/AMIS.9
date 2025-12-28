using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.AnnualProcurementPlans.Update.v1;

public sealed class UpdateAnnualProcurementPlanHandler(
    ILogger<UpdateAnnualProcurementPlanHandler> logger,
    [FromKeyedServices("catalog:annualProcurementPlans")] IRepository<AnnualProcurementPlanHeader> repository)
    : IRequestHandler<UpdateAnnualProcurementPlanCommand, UpdateAnnualProcurementPlanResponse>
{
    public async Task<UpdateAnnualProcurementPlanResponse> Handle(UpdateAnnualProcurementPlanCommand request, CancellationToken cancellationToken)
    {
        var plan = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        if (plan is null) throw new InvalidOperationException("Annual procurement plan not found.");

        plan.UpdateHeader(request.FiscalYear, request.BudgetType);
        await repository.UpdateAsync(plan, cancellationToken).ConfigureAwait(false);

        logger.LogInformation("annual procurement plan updated {AnnualProcurementPlanId}", plan.Id);
        return new UpdateAnnualProcurementPlanResponse(plan.Id);
    }
}
