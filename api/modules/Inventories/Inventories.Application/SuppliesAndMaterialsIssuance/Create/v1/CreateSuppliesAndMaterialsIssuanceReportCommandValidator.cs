using FluentValidation;

namespace AMIS.WebApi.Inventories.Application.SuppliesAndMaterialsIssuance.Create.v1;

public sealed class CreateSuppliesAndMaterialsIssuanceReportCommandValidator
    : AbstractValidator<CreateSuppliesAndMaterialsIssuanceReportCommand>
{
    public CreateSuppliesAndMaterialsIssuanceReportCommandValidator()
    {
        RuleFor(x => x.SmirNumber)
            .NotEmpty().WithMessage("SMIR number is required")
            .MaximumLength(50).WithMessage("SMIR number cannot exceed 50 characters");

        RuleFor(x => x.TransactionDate)
            .NotEmpty().WithMessage("Transaction date is required")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Transaction date cannot be in the future");

        RuleFor(x => x.RecipientName)
            .NotEmpty().WithMessage("Recipient name is required")
            .MaximumLength(200).WithMessage("Recipient name cannot exceed 200 characters");

        RuleFor(x => x.RecipientAddress)
            .NotEmpty().WithMessage("Recipient address is required")
            .MaximumLength(500).WithMessage("Recipient address cannot exceed 500 characters");

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

        RuleFor(x => x.IssuingOfficerName)
            .NotEmpty().WithMessage("Issuing officer name is required");

        RuleFor(x => x.AuthRecipientName)
            .NotEmpty().WithMessage("Recipient name in authorization is required");
    }
}

