using FluentValidation;

namespace AMIS.WebApi.Catalog.Application.AnnualProcurementPlans.Update.v1;

public sealed class UpdateAnnualProcurementPlanValidator : AbstractValidator<UpdateAnnualProcurementPlanCommand>
{
    public UpdateAnnualProcurementPlanValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.FiscalYear).GreaterThan(0);
        RuleFor(x => x.BudgetType).IsInEnum();
    }
}
