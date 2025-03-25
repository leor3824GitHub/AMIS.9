using FluentValidation;

namespace AMIS.WebApi.Catalog.Application.PurchaseItems.Update.v1;
public class UpdatePurchaseItemCommandValidator : AbstractValidator<UpdatePurchaseItemCommand>
{
    public UpdatePurchaseItemCommandValidator()
    {
        RuleFor(p => p.PurchaseId).NotEmpty();
        RuleFor(p => p.ProductId).NotEmpty();
    }
}
