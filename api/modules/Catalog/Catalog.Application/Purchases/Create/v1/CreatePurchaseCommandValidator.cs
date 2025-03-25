using FluentValidation;

namespace AMIS.WebApi.Catalog.Application.Purchases.Create.v1;
public class CreatePurchaseCommandValidator : AbstractValidator<CreatePurchaseCommand>
{
    public CreatePurchaseCommandValidator()
    {
        RuleFor(p => p.SupplierId).NotEmpty();
        RuleFor(p => p.PurchaseDate).NotEmpty();
    }
}
