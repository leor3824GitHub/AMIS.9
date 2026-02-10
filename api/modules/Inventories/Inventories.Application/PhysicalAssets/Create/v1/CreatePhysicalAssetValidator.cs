using FluentValidation;

namespace AMIS.WebApi.Inventories.Application.PhysicalAssets.Create.v1;

public sealed class CreatePhysicalAssetValidator : AbstractValidator<CreatePhysicalAssetCommand>
{
    public CreatePhysicalAssetValidator()
    {
        RuleFor(x => x.PropertyCode)
            .NotEmpty().WithMessage("Property code is required.");

        RuleFor(x => x.ProductId)
            .NotEqual(Guid.Empty).WithMessage("Product ID is required.");

        RuleFor(x => x.AcquisitionCost)
            .GreaterThan(0).WithMessage("Acquisition cost must be greater than zero.");

        RuleFor(x => x.AcquisitionDate)
            .NotEmpty().WithMessage("Acquisition date is required.")
            .Custom((date, context) =>
            {
                if (date.Date > DateTime.UtcNow.Date)
                {
                    context.AddFailure("Acquisition date cannot be in the future.");
                }
            });

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
    }
}
