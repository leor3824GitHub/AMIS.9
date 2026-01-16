using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.ProcurementPlans.Get.v1;

public sealed class GetProcurementPlanHandler(
    ILogger<GetProcurementPlanHandler> logger,
    [FromKeyedServices("catalog:procurementPlans")] IReadRepository<ProcurementPlanHeader> repository)
    : IRequestHandler<GetProcurementPlanRequest, GetProcurementPlanResponse?>
{
    public async Task<GetProcurementPlanResponse?> Handle(GetProcurementPlanRequest request, CancellationToken cancellationToken)
    {
        var plan = await repository.FirstOrDefaultAsync(new GetProcurementPlanSpec(request.Id), cancellationToken).ConfigureAwait(false);
        if (plan is null) return null;

        var response = new GetProcurementPlanResponse(
            plan.Id,
            plan.ControlNumber,
            plan.FiscalYear,
            plan.DepartmentId,
            plan.DepartmentName,
            plan.Status,
            plan.IsSupplemental,
            plan.BudgetType,
            plan.TotalBudget,
            plan.PreparedByUserId,
            plan.SubmissionDate,
            plan.ApprovedByUserId,
            plan.ApprovalDate,
            plan.RejectionReason,
            plan.Items
                .Select(i => new ProcurementPlanItemResponse(
                    i.Id,
                    i.PapCode,
                    i.Description,
                    i.ProjectType,
                    i.Quantity,
                    i.UnitOfMeasure,
                    i.UnitCost,
                    i.EstimatedBudget,
                    i.Mode,
                    i.IsEarlyProcurement,
                    i.ScheduleMonth,
                    i.FundingSource,
                    i.Remarks))
                .ToList());

        logger.LogInformation("Fetched procurement plan {ProcurementPlanId}", plan.Id);
        return response;
    }
}
