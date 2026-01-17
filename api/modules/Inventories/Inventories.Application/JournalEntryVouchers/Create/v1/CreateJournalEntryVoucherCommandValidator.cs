using FluentValidation;

namespace AMIS.WebApi.Inventories.Application.JournalEntryVouchers.Create.v1;

public class CreateJournalEntryVoucherCommandValidator : AbstractValidator<CreateJournalEntryVoucherCommand>
{
    public CreateJournalEntryVoucherCommandValidator()
    {
        RuleFor(x => x.Year)
            .GreaterThan(2000)
            .LessThanOrEqualTo(DateTime.UtcNow.Year)
            .WithMessage("Year must be valid");

        RuleFor(x => x.Month)
            .InclusiveBetween(1, 12)
            .WithMessage("Month must be between 1 and 12");
    }
}

