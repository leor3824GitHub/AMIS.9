using FluentValidation;

namespace AMIS.WebApi.Inventories.Application.AnnualProcurementPlans.Create.v1;

public sealed class CreateAnnualProcurementPlanValidator : AbstractValidator<CreateAnnualProcurementPlanCommand>
{
    public CreateAnnualProcurementPlanValidator()
    {
        RuleFor(x => x.ControlNumber).NotEmpty().MaximumLength(50);
        RuleFor(x => x.FiscalYear).GreaterThan(0);
        RuleFor(x => x.PreparedByUserId).NotEmpty();
        RuleFor(x => x.BudgetType).IsInEnum();
    }
}

