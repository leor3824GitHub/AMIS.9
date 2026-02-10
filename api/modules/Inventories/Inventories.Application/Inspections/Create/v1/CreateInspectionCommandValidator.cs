using FluentValidation;
using AMIS.WebApi.Inventories.Domain;

namespace AMIS.WebApi.Inventories.Application.Inspections.Create.v1;

public class CreateInspectionCommandValidator : AbstractValidator<CreateInspectionCommand>
{
    public CreateInspectionCommandValidator()
    {
        RuleFor(x => x.Type).IsInEnum().WithMessage("InspectionType must be NewDelivery, AssetReturn, or Repair");
        RuleFor(x => x.EmployeeId).NotEmpty().WithMessage("EmployeeId is required");

        // Type-specific validation
        RuleFor(x => x.PurchaseId)
            .NotEmpty()
            .When(x => x.Type == InspectionType.NewDelivery)
            .WithMessage("PurchaseId is required for NewDelivery inspections");

        RuleFor(x => x.PhysicalAssetId)
            .NotEmpty()
            .When(x => x.Type == InspectionType.AssetReturn || x.Type == InspectionType.Repair)
            .WithMessage("PhysicalAssetId is required for AssetReturn and Repair inspections");

        // Items validation
        RuleForEach(x => x.Items).ChildRules(items =>
        {
            items.RuleFor(i => i.PurchaseItemId).NotEmpty().WithMessage("PurchaseItemId is required");
            items.RuleFor(i => i.QtyInspected).GreaterThan(0).WithMessage("QtyInspected must be greater than 0");
            items.RuleFor(i => i.QtyPassed).GreaterThanOrEqualTo(0).WithMessage("QtyPassed cannot be negative");
            items.RuleFor(i => i.QtyFailed).GreaterThanOrEqualTo(0).WithMessage("QtyFailed cannot be negative");
            items.RuleFor(i => i).Must(i => i.QtyPassed + i.QtyFailed == i.QtyInspected)
                .WithMessage("QtyPassed + QtyFailed must equal QtyInspected");
        });
    }
}


