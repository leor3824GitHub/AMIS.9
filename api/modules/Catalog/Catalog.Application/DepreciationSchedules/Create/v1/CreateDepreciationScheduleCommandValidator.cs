using FluentValidation;

namespace AMIS.WebApi.Catalog.Application.DepreciationSchedules.Create.v1;

public class CreateDepreciationScheduleCommandValidator : AbstractValidator<CreateDepreciationScheduleCommand>
{
    public CreateDepreciationScheduleCommandValidator()
    {
        RuleFor(x => x.AssetId)
            .NotEmpty()
            .WithMessage("Asset ID is required");

        RuleFor(x => x.Year)
            .GreaterThan(2000)
            .LessThanOrEqualTo(DateTime.UtcNow.Year)
            .WithMessage("Year must be valid");

        RuleFor(x => x.Month)
            .InclusiveBetween(1, 12)
            .WithMessage("Month must be between 1 and 12");

        RuleFor(x => x.MonthlyDepreciationAmount)
            .GreaterThan(0)
            .WithMessage("Monthly depreciation amount must be greater than zero");
    }
}
