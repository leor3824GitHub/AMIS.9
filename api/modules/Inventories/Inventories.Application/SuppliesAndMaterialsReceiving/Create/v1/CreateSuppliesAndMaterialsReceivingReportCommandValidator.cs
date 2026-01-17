using FluentValidation;

namespace AMIS.WebApi.Inventories.Application.SuppliesAndMaterialsReceiving.Create.v1;

public sealed class CreateSuppliesAndMaterialsReceivingReportCommandValidator
    : AbstractValidator<CreateSuppliesAndMaterialsReceivingReportCommand>
{
    public CreateSuppliesAndMaterialsReceivingReportCommandValidator()
    {
        RuleFor(x => x.SmrrNumber)
            .NotEmpty().WithMessage("SMRR number is required")
            .MaximumLength(50).WithMessage("SMRR number cannot exceed 50 characters");

        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("Location is required")
            .MaximumLength(100).WithMessage("Location cannot exceed 100 characters");

        RuleFor(x => x.SourceName)
            .NotEmpty().WithMessage("Source name is required")
            .MaximumLength(200).WithMessage("Source name cannot exceed 200 characters");

        RuleFor(x => x.SourceAddress)
            .NotEmpty().WithMessage("Source address is required")
            .MaximumLength(500).WithMessage("Source address cannot exceed 500 characters");

        RuleFor(x => x.ReceivingDate)
            .NotEmpty().WithMessage("Receiving date is required")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Receiving date cannot be in the future");

        RuleFor(x => x.TransactionType)
            .NotEmpty().WithMessage("Transaction type is required")
            .Must(t => t is "Purchase" or "Transfer" or "Donation" or "Others")
            .WithMessage("Transaction type must be Purchase, Transfer, Donation, or Others");

        RuleFor(x => x.LineItems)
            .NotEmpty().WithMessage("At least one line item is required")
            .ForEach(item =>
                item
                    .Must(i => !string.IsNullOrWhiteSpace(i.Name)).WithMessage("Item name is required")
                    .Must(i => i.Quantity > 0).WithMessage("Item quantity must be greater than zero")
                    .Must(i => i.UnitCost >= 0).WithMessage("Unit cost cannot be negative"));

        RuleFor(x => x.ReceivedByName)
            .NotEmpty().WithMessage("Received by name is required");

        RuleFor(x => x.NotedByName)
            .NotEmpty().WithMessage("Noted by name is required");
    }
}

