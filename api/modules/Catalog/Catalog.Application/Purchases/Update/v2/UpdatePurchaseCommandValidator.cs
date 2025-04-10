using FluentValidation;

namespace AMIS.WebApi.Catalog.Application.Purchases.Update.v2;
public class UpdatePurchaseCommandValidator : AbstractValidator<UpdatePurchaseCommand>
{
    public UpdatePurchaseCommandValidator()
    {
        RuleFor(p => p.SupplierId).NotEmpty();
        RuleFor(p => p.PurchaseDate).NotEmpty();
    }
}
