using AMIS.WebApi.Catalog.Domain;

namespace AMIS.WebApi.Catalog.Application.AnnualProcurementPlans.Search.v1;

public sealed record AnnualProcurementPlanListItemResponse(
    Guid Id,
    string ControlNumber,
    int FiscalYear,
    AnnualProcurementPlanStatus Status,
    BudgetType BudgetType,
    decimal TotalBudget);
