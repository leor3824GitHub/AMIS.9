using FluentValidation;

namespace AMIS.WebApi.Catalog.Application.Issuances.UpdateWithItems.v1;

public sealed class UpdateIssuanceWithItemsValidator : AbstractValidator<UpdateIssuanceWithItemsCommand>
{
    public UpdateIssuanceWithItemsValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.EmployeeId).NotEmpty();
        RuleFor(x => x.TotalAmount).GreaterThanOrEqualTo(0);

        RuleForEach(x => x.Items).ChildRules(items =>
        {
            items.RuleFor(i => i.Qty).GreaterThan(0);
            items.RuleFor(i => i.UnitPrice).GreaterThanOrEqualTo(0);
            items.RuleFor(i => i.ProductId).NotEmpty();
        });
    }
}