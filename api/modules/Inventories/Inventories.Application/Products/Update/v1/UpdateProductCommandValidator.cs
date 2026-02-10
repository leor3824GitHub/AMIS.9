using FluentValidation;

namespace AMIS.WebApi.Inventories.Application.Products.Update.v1;
public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(p => p.Name).NotEmpty().MinimumLength(2).MaximumLength(75);
    }
}

