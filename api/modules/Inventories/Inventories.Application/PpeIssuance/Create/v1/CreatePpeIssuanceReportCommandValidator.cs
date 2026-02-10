using FluentValidation;

namespace AMIS.WebApi.Inventories.Application.PpeIssuance.Create.v1;

public sealed class CreatePpeIssuanceReportCommandValidator : AbstractValidator<CreatePpeIssuanceReportCommand>
{
    public CreatePpeIssuanceReportCommandValidator()
    {
        RuleFor(x => x.IRNumber)
            .NotEmpty().WithMessage("IR number is required.")
            .MaximumLength(50).WithMessage("IR number cannot exceed 50 characters.");

        RuleFor(x => x.IssuedTo)
            .NotEmpty().WithMessage("Issued to is required.")
            .MaximumLength(255).WithMessage("Issued to cannot exceed 255 characters.");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required.")
            .MaximumLength(500).WithMessage("Address cannot exceed 500 characters.");

        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Type is required.")
            .MaximumLength(50).WithMessage("Type cannot exceed 50 characters.");

        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Date cannot be in the future.");

        RuleFor(x => x.LineItems)
            .NotEmpty().WithMessage("At least one line item is required.")
            .Must(items => items.Count > 0).WithMessage("Line items collection cannot be empty.");
    }
}

