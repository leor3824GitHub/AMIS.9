using FluentValidation;

namespace AMIS.WebApi.Inventories.Application.PpeReceiving.Create.v1;

public sealed class CreatePpeReceivingReportCommandValidator : AbstractValidator<CreatePpeReceivingReportCommand>
{
    public CreatePpeReceivingReportCommandValidator()
    {
        RuleFor(x => x.ReportNumber).NotEmpty().WithMessage("Report number is required.");
        RuleFor(x => x.SourceName).NotEmpty().WithMessage("Source name is required.");
        RuleFor(x => x.SourceAddress).NotEmpty().WithMessage("Source address is required.");
        RuleFor(x => x.ReceiptType).NotEmpty().WithMessage("Receipt type is required.");
        RuleFor(x => x.LineItems).NotEmpty().WithMessage("At least one line item is required.");
        RuleForEach(x => x.LineItems)
            .ChildRules(item =>
            {
                item.RuleFor(li => li.Location)
                    .NotEmpty()
                    .WithMessage("Line item location is required.");
            });
    }
}

