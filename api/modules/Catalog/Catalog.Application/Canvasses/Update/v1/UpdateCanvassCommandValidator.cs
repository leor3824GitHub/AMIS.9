using FluentValidation;

namespace AMIS.WebApi.Catalog.Application.Canvasses.Update.v1;

public sealed class UpdateCanvassCommandValidator : AbstractValidator<UpdateCanvassCommand>
{
    public UpdateCanvassCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Canvass ID is required.");

        RuleFor(x => x.ItemDescription)
            .NotEmpty()
            .WithMessage("Item description is required.")
            .MaximumLength(512)
            .WithMessage("Item description must not exceed 512 characters.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero.");

        RuleFor(x => x.Unit)
            .NotEmpty()
            .WithMessage("Unit is required.")
            .MaximumLength(50)
            .WithMessage("Unit must not exceed 50 characters.");

        RuleFor(x => x.QuotedPrice)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Quoted price cannot be negative.");

        RuleFor(x => x.Remarks)
            .MaximumLength(1024)
            .When(x => !string.IsNullOrEmpty(x.Remarks))
            .WithMessage("Remarks must not exceed 1024 characters.");
    }
}
