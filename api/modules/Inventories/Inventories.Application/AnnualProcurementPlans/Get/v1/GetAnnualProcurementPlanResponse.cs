using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.ValueObjects;

namespace AMIS.WebApi.Inventories.Application.AnnualProcurementPlans.Get.v1;

public sealed record GetAnnualProcurementPlanResponse(
    Guid Id,
    string ControlNumber,
    int FiscalYear,
    AnnualProcurementPlanStatus Status,
    BudgetType BudgetType,
    decimal TotalBudget,
    Guid PreparedByUserId,
    DateTimeOffset? SubmissionDate,
    Guid? ApprovedByUserId,
    DateTimeOffset? ApprovalDate,
    string? RejectionReason,
    List<AnnualProcurementPlanItemResponse> Items);

public sealed record AnnualProcurementPlanItemResponse(
    Guid Id,
    Guid DepartmentId,
    string DepartmentName,
    string? PapCode,
    string Description,
    ProjectType ProjectType,
    int Quantity,
    string UnitOfMeasure,
    decimal UnitCost,
    decimal EstimatedBudget,
    string Mode,
    bool IsEarlyProcurement,
    string ScheduleMonth,
    string FundingSource,
    string? Remarks);

