using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.ValueObjects;

namespace AMIS.WebApi.Inventories.Application.AnnualProcurementPlans.Search.v1;

public sealed record AnnualProcurementPlanListItemResponse(
    Guid Id,
    string ControlNumber,
    int FiscalYear,
    AnnualProcurementPlanStatus Status,
    BudgetType BudgetType,
    decimal TotalBudget);

