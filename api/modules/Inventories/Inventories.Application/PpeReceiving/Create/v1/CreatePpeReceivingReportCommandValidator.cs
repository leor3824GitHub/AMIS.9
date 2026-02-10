using FluentValidation;

namespace AMIS.WebApi.Inventories.Application.PpeReceiving.Create.v1;

public sealed class CreatePpeReceivingReportCommandValidator : AbstractValidator<CreatePpeReceivingReportCommand>
{
    public CreatePpeReceivingReportCommandValidator()
    {
        RuleFor(x => x.RRNumber).NotEmpty().WithMessage("Report number is required.");
        RuleFor(x => x.ReceivedFrom).NotEmpty().WithMessage("Received from is required.");
        RuleFor(x => x.Address).NotEmpty().WithMessage("Address is required.");
        RuleFor(x => x.Type).NotEmpty().WithMessage("Receipt type is required.");
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

