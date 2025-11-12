using FluentValidation;

namespace AMIS.WebApi.Catalog.Application.Purchases.UpdateWithItems.v1;

public sealed class UpdatePurchaseWithItemsValidator : AbstractValidator<UpdatePurchaseWithItemsCommand>
{
    public UpdatePurchaseWithItemsValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.TotalAmount).GreaterThanOrEqualTo(0);

        RuleForEach(x => x.Items).ChildRules(items =>
        {
            items.RuleFor(i => i.Qty).GreaterThan(0);
            items.RuleFor(i => i.UnitPrice).GreaterThanOrEqualTo(0);
            items.When(i => !i.Id.HasValue, () =>
            {
                items.RuleFor(i => i.ProductId).NotEmpty();
            });
        });
    }
}
