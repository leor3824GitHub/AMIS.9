using FluentValidation;

namespace AMIS.WebApi.Catalog.Application.Inventories.Update.v1;
public class UpdateInventoryCommandValidator : AbstractValidator<UpdateInventoryCommand>
{
    public UpdateInventoryCommandValidator()
    {
        RuleFor(p => p.ProductId).NotEmpty();
    }
}
