using FluentValidation;

namespace AMIS.WebApi.Catalog.Application.ProcurementPlans.Items.v1;

public sealed class AddProcurementPlanItemValidator : AbstractValidator<AddProcurementPlanItemCommand>
{
    public AddProcurementPlanItemValidator()
    {
        RuleFor(x => x.PlanId).NotEmpty();
        RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Quantity).GreaterThan(0);
        RuleFor(x => x.UnitOfMeasure).NotEmpty().MaximumLength(50);
        RuleFor(x => x.UnitCost).GreaterThan(0);
        RuleFor(x => x.Mode).NotEmpty().MaximumLength(100);
        RuleFor(x => x.ScheduleMonth).NotEmpty().MaximumLength(20);
        RuleFor(x => x.FundingSource).NotEmpty().MaximumLength(200);
        RuleFor(x => x.PapCode).MaximumLength(50);
        RuleFor(x => x.Remarks).MaximumLength(1000);
    }
}
