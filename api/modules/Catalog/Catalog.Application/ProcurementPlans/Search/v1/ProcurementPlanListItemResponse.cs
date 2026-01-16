using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Application.ProcurementPlans.Search.v1;

public sealed record ProcurementPlanListItemResponse(
    Guid Id,
    string ControlNumber,
    int FiscalYear,
    Guid DepartmentId,
    string DepartmentName,
    ProcurementPlanStatus Status,
    bool IsSupplemental,
    BudgetType BudgetType,
    decimal TotalBudget);
