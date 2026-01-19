using FluentValidation;

namespace AMIS.WebApi.Inventories.Application.PpeIssuance.Create.v1;

public sealed class CreatePpeIssuanceReportCommandValidator : AbstractValidator<CreatePpeIssuanceReportCommand>
{
    public CreatePpeIssuanceReportCommandValidator()
    {
        RuleFor(x => x.ReportNumber)
            .NotEmpty().WithMessage("Report number is required.")
            .MaximumLength(50).WithMessage("Report number cannot exceed 50 characters.");

        RuleFor(x => x.RecipientName)
            .NotEmpty().WithMessage("Recipient name is required.")
            .MaximumLength(255).WithMessage("Recipient name cannot exceed 255 characters.");

        RuleFor(x => x.RecipientAddress)
            .NotEmpty().WithMessage("Recipient address is required.")
            .MaximumLength(500).WithMessage("Recipient address cannot exceed 500 characters.");

        RuleFor(x => x.IssuanceType)
            .NotEmpty().WithMessage("Issuance type is required.")
            .MaximumLength(50).WithMessage("Issuance type cannot exceed 50 characters.");

        RuleFor(x => x.IssuanceDate)
            .NotEmpty().WithMessage("Issuance date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Issuance date cannot be in the future.");

        RuleFor(x => x.LineItems)
            .NotEmpty().WithMessage("At least one line item is required.")
            .Must(items => items.Count > 0).WithMessage("Line items collection cannot be empty.");
    }
}

