using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.AnnualProcurementPlans.Get.v1;

public sealed class GetAnnualProcurementPlanHandler(
    ILogger<GetAnnualProcurementPlanHandler> logger,
    [FromKeyedServices("inventories:annualProcurementPlans")] IReadRepository<AnnualProcurementPlanHeader> repository)
    : IRequestHandler<GetAnnualProcurementPlanRequest, GetAnnualProcurementPlanResponse?>
{
    public async Task<GetAnnualProcurementPlanResponse?> Handle(GetAnnualProcurementPlanRequest request, CancellationToken cancellationToken)
    {
        var plan = await repository.FirstOrDefaultAsync(new GetAnnualProcurementPlanSpec(request.Id), cancellationToken).ConfigureAwait(false);
        if (plan is null) return null;

        var response = new GetAnnualProcurementPlanResponse(
            plan.Id,
            plan.ControlNumber,
            plan.FiscalYear,
            plan.Status,
            plan.BudgetType,
            plan.TotalBudget,
            plan.PreparedByUserId,
            plan.SubmissionDate,
            plan.ApprovedByUserId,
            plan.ApprovalDate,
            plan.RejectionReason,
            plan.Items
                .Select(i => new AnnualProcurementPlanItemResponse(
                    i.Id,
                    i.DepartmentId,
                    i.DepartmentName,
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

        logger.LogInformation("Fetched annual procurement plan {AnnualProcurementPlanId}", plan.Id);
        return response;
    }
}

