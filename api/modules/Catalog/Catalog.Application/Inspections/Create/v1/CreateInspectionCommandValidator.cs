using FluentValidation;

namespace AMIS.WebApi.Catalog.Application.Inspections.Create.v1;

public class CreateInspectionCommandValidator : AbstractValidator<CreateInspectionCommand>
{
    public CreateInspectionCommandValidator()
    {
        RuleFor(c => c.PurchaseId).NotEmpty();
        RuleFor(c => c.InspectorId).NotEmpty();
        RuleFor(c => c.InspectionDate).NotEmpty();

        RuleForEach(c => c.Items).ChildRules(items =>
        {
            items.RuleFor(i => i.PurchaseItemId).NotEmpty();
            items.RuleFor(i => i.QtyInspected).GreaterThan(0);
        });
    }
}
