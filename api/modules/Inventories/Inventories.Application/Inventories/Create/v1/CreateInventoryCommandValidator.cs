using FluentValidation;

namespace AMIS.WebApi.Inventories.Application.Inventories.Create.v1;
public class CreateInventoryCommandValidator : AbstractValidator<CreateInventoryCommand>
{
    public CreateInventoryCommandValidator()
    {
        RuleFor(p => p.ProductId).NotEmpty();
    }
}

