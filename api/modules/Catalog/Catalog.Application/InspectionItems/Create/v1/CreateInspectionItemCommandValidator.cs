using FluentValidation;

namespace AMIS.WebApi.Catalog.Application.Inspections.CreateItem.v1;

public class CreateInspectionItemCommandValidator : AbstractValidator<CreateInspectionItemCommand>
{
    public CreateInspectionItemCommandValidator()
    {
        RuleFor(x => x.InspectionId).NotEmpty();
        RuleFor(x => x.PurchaseItemId).NotEmpty();
        RuleFor(x => x.QtyInspected).GreaterThan(0);
        RuleFor(x => x.QtyPassed).GreaterThanOrEqualTo(0);
        RuleFor(x => x.QtyFailed).GreaterThanOrEqualTo(0);
        RuleFor(x => x.QtyPassed + x.QtyFailed)
            .Equal(x => x.QtyInspected)
            .WithMessage("Sum of QtyPassed and QtyFailed must equal QtyInspected.");
    }
}
