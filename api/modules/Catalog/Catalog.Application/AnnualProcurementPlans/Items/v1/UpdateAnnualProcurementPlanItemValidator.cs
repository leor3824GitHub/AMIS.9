using FluentValidation;

namespace AMIS.WebApi.Catalog.Application.AnnualProcurementPlans.Items.v1;

public sealed class UpdateAnnualProcurementPlanItemValidator : AbstractValidator<UpdateAnnualProcurementPlanItemCommand>
{
    public UpdateAnnualProcurementPlanItemValidator()
    {
        RuleFor(x => x.PlanId).NotEmpty();
        RuleFor(x => x.ItemId).NotEmpty();
        RuleFor(x => x.DepartmentId).NotEmpty();
        RuleFor(x => x.Description).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Quantity).GreaterThan(0);
        RuleFor(x => x.UnitOfMeasure).NotEmpty().MaximumLength(50);
        RuleFor(x => x.UnitCost).GreaterThan(0);
        RuleFor(x => x.Mode).NotEmpty().MaximumLength(100);
        RuleFor(x => x.ScheduleMonth).NotEmpty().MaximumLength(20);
        RuleFor(x => x.FundingSource).NotEmpty().MaximumLength(200);
    }
}
