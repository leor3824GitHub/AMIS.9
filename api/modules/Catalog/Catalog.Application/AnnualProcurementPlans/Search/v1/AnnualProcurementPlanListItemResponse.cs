using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Application.AnnualProcurementPlans.Search.v1;

public sealed record AnnualProcurementPlanListItemResponse(
    Guid Id,
    string ControlNumber,
    int FiscalYear,
    AnnualProcurementPlanStatus Status,
    BudgetType BudgetType,
    decimal TotalBudget);
