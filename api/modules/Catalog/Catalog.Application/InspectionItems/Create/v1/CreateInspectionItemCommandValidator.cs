using FluentValidation;

namespace AMIS.WebApi.Catalog.Application.Inspections.CreateItem.v1;

public class CreateInspectionItemCommandValidator : AbstractValidator<CreateInspectionItemCommand>
{
    public CreateInspectionItemCommandValidator()
    {
        RuleFor(x => x.InspectionId).NotEmpty();
        RuleFor(x => x.PurchaseItemId).NotEmpty();
        RuleFor(x => x.QuantityInspected).GreaterThan(0);
        RuleFor(x => x.QuantityPassed).GreaterThanOrEqualTo(0);
        RuleFor(x => x.QuantityFailed).GreaterThanOrEqualTo(0);
        RuleFor(x => x.QuantityPassed + x.QuantityFailed)
            .Equal(x => x.QuantityInspected)
            .WithMessage("Sum of QuantityPassed and QuantityFailed must equal QuantityInspected.");
    }
}
