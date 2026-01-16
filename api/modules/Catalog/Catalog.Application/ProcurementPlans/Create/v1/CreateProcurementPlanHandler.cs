using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.ProcurementPlans.Create.v1;

public sealed class CreateProcurementPlanHandler(
    ILogger<CreateProcurementPlanHandler> logger,
    [FromKeyedServices("catalog:procurementPlans")] IRepository<ProcurementPlanHeader> repository)
    : IRequestHandler<CreateProcurementPlanCommand, CreateProcurementPlanResponse>
{
    public async Task<CreateProcurementPlanResponse> Handle(CreateProcurementPlanCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var plan = ProcurementPlanHeader.Create(
            request.ControlNumber,
            request.FiscalYear,
            request.DepartmentId,
            request.DepartmentName,
            request.IsSupplemental,
            request.BudgetType,
            request.PreparedByUserId);

        await repository.AddAsync(plan, cancellationToken).ConfigureAwait(false);
        logger.LogInformation("procurement plan created {ProcurementPlanId}", plan.Id);
        return new CreateProcurementPlanResponse(plan.Id);
    }
}
