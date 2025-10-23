using FluentValidation;

namespace AMIS.WebApi.Catalog.Application.InspectionItems.Update.v1;

public class UpdateInspectionItemCommandValidator : AbstractValidator<UpdateInspectionItemCommand>
{
    public UpdateInspectionItemCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.InspectionId).NotEmpty();
        RuleFor(x => x.PurchaseItemId).NotEmpty();
        RuleFor(x => x.QtyInspected).GreaterThanOrEqualTo(0);
        RuleFor(x => x.QtyPassed).GreaterThanOrEqualTo(0);
        RuleFor(x => x.QtyFailed).GreaterThanOrEqualTo(0);
        RuleFor(x => x)
            .Must(x => x.QtyPassed + x.QtyFailed == x.QtyInspected)
            .WithMessage("QtyPassed + QtyFailed must equal QtyInspected");
    }
}
