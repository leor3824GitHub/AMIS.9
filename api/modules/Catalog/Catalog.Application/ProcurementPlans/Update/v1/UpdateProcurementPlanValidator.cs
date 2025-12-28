using FluentValidation;

namespace AMIS.WebApi.Catalog.Application.ProcurementPlans.Update.v1;

public sealed class UpdateProcurementPlanValidator : AbstractValidator<UpdateProcurementPlanCommand>
{
    public UpdateProcurementPlanValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.DepartmentId).NotEmpty();
        RuleFor(x => x.DepartmentName).MaximumLength(200);
    }
}
