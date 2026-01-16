using FluentValidation;

namespace AMIS.WebApi.Catalog.Application.ProcurementPlans.Create.v1;

public sealed class CreateProcurementPlanValidator : AbstractValidator<CreateProcurementPlanCommand>
{
    public CreateProcurementPlanValidator()
    {
        RuleFor(x => x.ControlNumber).NotEmpty().MaximumLength(50);
        RuleFor(x => x.FiscalYear).GreaterThan(0);
        RuleFor(x => x.DepartmentId).NotEmpty();
        RuleFor(x => x.DepartmentName).MaximumLength(200);
        RuleFor(x => x.PreparedByUserId).NotEmpty();
    }
}
