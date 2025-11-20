using FluentValidation;

namespace AMIS.WebApi.Catalog.Application.PurchaseRequests.Create.v1;

public sealed class CreatePurchaseRequestCommandValidator : AbstractValidator<CreatePurchaseRequestCommand>
{
    public CreatePurchaseRequestCommandValidator()
    {
        RuleFor(x => x.RequestedBy).NotEmpty();
        RuleFor(x => x.Purpose).NotEmpty().MaximumLength(512);
        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.Qty).GreaterThan(0);
            item.RuleFor(i => i.Unit).NotEmpty().MaximumLength(50);
            item.RuleFor(i => i.Description).MaximumLength(512);
        }).When(x => x.Items != null && x.Items.Count > 0);
    }
}
