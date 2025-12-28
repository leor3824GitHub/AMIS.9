using FluentValidation;

namespace AMIS.WebApi.Catalog.Application.ProcurementProjects.Update.v1;

public sealed class UpdateProcurementProjectValidator : AbstractValidator<UpdateProcurementProjectCommand>
{
    public UpdateProcurementProjectValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.ProjectTitle).NotEmpty().MaximumLength(500);
        RuleFor(x => x.PmoEndUser).NotEmpty().MaximumLength(200);
        RuleFor(x => x.FundSource).NotEmpty().MaximumLength(200);
        RuleFor(x => x.PapCode).MaximumLength(50);
        RuleFor(x => x.Remarks).MaximumLength(1000);

        // Budget validation: total must equal mooe + co
        RuleFor(x => x)
            .Must(x => x.TotalAmount == x.MooeAmount + x.CoAmount)
            .WithMessage("Total amount must equal MOOE amount + CO amount.");

        RuleFor(x => x.TotalAmount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.MooeAmount).GreaterThanOrEqualTo(0);
        RuleFor(x => x.CoAmount).GreaterThanOrEqualTo(0);
    }
}
