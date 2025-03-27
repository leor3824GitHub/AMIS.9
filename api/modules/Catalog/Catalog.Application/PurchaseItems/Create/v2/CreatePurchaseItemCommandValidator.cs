using FluentValidation;

namespace AMIS.WebApi.Catalog.Application.PurchaseItems.Create.v2;
public class CreatePurchaseItemCommandValidator : AbstractValidator<CreatePurchaseItemCommand>
{
    public CreatePurchaseItemCommandValidator()
    {
        RuleFor(p => p.PurchaseId).NotEmpty();
        RuleFor(p => p.ProductId).NotEmpty();
    }
}
