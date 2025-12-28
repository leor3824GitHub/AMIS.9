using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.AnnualProcurementPlans.Create.v1;

public sealed class CreateAnnualProcurementPlanHandler(
    ILogger<CreateAnnualProcurementPlanHandler> logger,
    [FromKeyedServices("catalog:annualProcurementPlans")] IRepository<AnnualProcurementPlanHeader> repository)
    : IRequestHandler<CreateAnnualProcurementPlanCommand, CreateAnnualProcurementPlanResponse>
{
    public async Task<CreateAnnualProcurementPlanResponse> Handle(CreateAnnualProcurementPlanCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var plan = AnnualProcurementPlanHeader.Create(
            request.ControlNumber,
            request.FiscalYear,
            request.BudgetType,
            request.PreparedByUserId);

        await repository.AddAsync(plan, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("annual procurement plan created {AnnualProcurementPlanId}", plan.Id);
        return new CreateAnnualProcurementPlanResponse(plan.Id);
    }
}
