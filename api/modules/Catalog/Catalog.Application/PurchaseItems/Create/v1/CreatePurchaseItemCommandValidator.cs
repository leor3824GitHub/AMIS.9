using FluentValidation;

namespace AMIS.WebApi.Catalog.Application.PurchaseItems.Create.v1;
public class CreatePurchaseItemCommandValidator : AbstractValidator<CreatePurchaseItemCommand>
{
    public CreatePurchaseItemCommandValidator()
    {
        RuleFor(p => p.PurchaseId).NotEmpty();
        RuleFor(p => p.ProductId).NotEmpty();
    }
}
