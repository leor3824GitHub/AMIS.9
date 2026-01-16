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

            item.RuleFor(i => i)
                .Must(i => (i.ProductId is not null) ^ !string.IsNullOrWhiteSpace(i.ManualProductName))
                .WithMessage("Exactly one of ProductId or ManualProductName must be provided.");

            item.When(i => i.ManualProductName is not null, () =>
            {
                item.RuleFor(i => i.ManualProductName).NotEmpty().MaximumLength(256);
            });
        }).When(x => x.Items != null && x.Items.Count > 0);
    }
}
