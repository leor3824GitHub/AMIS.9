using FluentValidation;

namespace AMIS.WebApi.Catalog.Application.Inspections.Create.v1;

public class CreateInspectionCommandValidator : AbstractValidator<CreateInspectionCommand>
{
    public CreateInspectionCommandValidator()
    {
        RuleFor(x => x.InspectionDate).NotEmpty();
        RuleFor(x => x.InspectorId).NotEmpty();
        RuleFor(x => x.PurchaseId).NotEmpty();

        RuleForEach(x => x.Items).ChildRules(items =>
        {
            items.RuleFor(i => i.PurchaseItemId).NotEmpty();
            items.RuleFor(i => i.QtyInspected).GreaterThanOrEqualTo(0);
            items.RuleFor(i => i.QtyPassed).GreaterThanOrEqualTo(0);
            items.RuleFor(i => i.QtyFailed).GreaterThanOrEqualTo(0);
            items.RuleFor(i => i).Must(i => i.QtyPassed + i.QtyFailed == i.QtyInspected)
                .WithMessage("QtyPassed + QtyFailed must equal QtyInspected");
        });
    }
}
