using FluentValidation;

namespace AMIS.Modules.Catalog.Application.MaterialsIssuance.Features.Create.v1;

public class CreateSuppliesAndMaterialsIssuanceReportValidator
    : AbstractValidator<CreateSuppliesAndMaterialsIssuanceReportCommand>
{
    public CreateSuppliesAndMaterialsIssuanceReportValidator()
    {
        RuleFor(x => x.SmirNumber)
            .NotEmpty().WithMessage("SMIR number is required")
            .MaximumLength(50).WithMessage("SMIR number cannot exceed 50 characters");

        RuleFor(x => x.TransactionDate)
            .NotEmpty().WithMessage("Transaction date is required")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Transaction date cannot be in the future");

        RuleFor(x => x.Recipient)
            .NotNull().WithMessage("Recipient information is required")
            .Must(r => !string.IsNullOrWhiteSpace(r.Name)).WithMessage("Recipient name is required")
            .Must(r => !string.IsNullOrWhiteSpace(r.Address)).WithMessage("Recipient address is required");

        RuleFor(x => x.IssuanceReason)
            .NotEmpty().WithMessage("Issuance reason is required")
            .Must(r => r is "Sale" or "Transfer" or "Donation")
            .WithMessage("Issuance reason must be Sale, Transfer, or Donation");

        RuleFor(x => x.LineItems)
            .NotEmpty().WithMessage("At least one line item is required")
            .ForEach(item =>
                item
                    .Must(i => !string.IsNullOrWhiteSpace(i.Name)).WithMessage("Item name is required")
                    .Must(i => i.Quantity > 0).WithMessage("Item quantity must be greater than zero")
                    .Must(i => i.UnitCost >= 0).WithMessage("Unit cost cannot be negative"));

        RuleFor(x => x.Authorization)
            .NotNull().WithMessage("Authorization information is required")
            .Must(a => !string.IsNullOrWhiteSpace(a.IssuingOfficerName)).WithMessage("Issuing officer name is required")
            .Must(a => !string.IsNullOrWhiteSpace(a.RecipientName)).WithMessage("Recipient name in authorization is required");
    }
}
