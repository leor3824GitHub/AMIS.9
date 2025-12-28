using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.ProcurementPlans.Get.v1;

public sealed record ProcurementPlanItemResponse(
    Guid Id,
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

public sealed record GetProcurementPlanResponse(
    Guid Id,
    string ControlNumber,
    int FiscalYear,
    Guid DepartmentId,
    string DepartmentName,
    ProcurementPlanStatus Status,
    bool IsSupplemental,
    BudgetType BudgetType,
    decimal TotalBudget,
    Guid PreparedByUserId,
    DateTimeOffset? SubmissionDate,
    Guid? ApprovedByUserId,
    DateTimeOffset? ApprovalDate,
    string? RejectionReason,
    IReadOnlyList<ProcurementPlanItemResponse> Items);
