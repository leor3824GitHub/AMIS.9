using FluentValidation;

namespace AMIS.WebApi.Inventories.Application.DepreciationSchedules.Post.v1;

public class PostDepreciationScheduleCommandValidator : AbstractValidator<PostDepreciationScheduleCommand>
{
    public PostDepreciationScheduleCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Depreciation schedule ID is required");

        RuleFor(x => x.JournalEntryVoucherId)
            .NotEmpty()
            .WithMessage("Journal entry voucher ID is required");
    }
}

