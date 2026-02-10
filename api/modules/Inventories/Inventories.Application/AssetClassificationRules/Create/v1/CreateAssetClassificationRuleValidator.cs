using FluentValidation;

namespace AMIS.WebApi.Inventories.Application.AssetClassificationRules.Create.v1;

public class CreateAssetClassificationRuleValidator : AbstractValidator<CreateAssetClassificationRuleCommand>
{
    public CreateAssetClassificationRuleValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

        RuleFor(x => x.MinimumCost)
            .GreaterThanOrEqualTo(0).WithMessage("Minimum cost must be greater than or equal to 0.");

        RuleFor(x => x.MaximumCost)
            .GreaterThan(x => x.MinimumCost)
            .WithMessage("Maximum cost must be greater than minimum cost.");

        RuleFor(x => x.EffectiveDate)
            .LessThan(x => x.ExpiryDate)
            .WithMessage("Effective date must be before expiry date.");

        RuleFor(x => x.RCAAccountCode)
            .NotEmpty().WithMessage("RCA Account Code is required.")
            .MaximumLength(50).WithMessage("RCA Account Code must not exceed 50 characters.");

        RuleFor(x => x.Priority)
            .GreaterThan(0).WithMessage("Priority must be greater than 0.");
    }
}
