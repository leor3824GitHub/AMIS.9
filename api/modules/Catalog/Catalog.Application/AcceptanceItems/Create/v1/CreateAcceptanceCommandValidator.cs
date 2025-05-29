using FluentValidation;

namespace AMIS.WebApi.Catalog.Application.Acceptances.Create.v1;

public class CreateAcceptanceCommandValidator : AbstractValidator<CreateAcceptanceCommand>
{
    public CreateAcceptanceCommandValidator()
    {
        RuleFor(c => c.PurchaseId).NotEmpty();
        RuleFor(c => c.AcceptorId).NotEmpty();
        RuleFor(c => c.AcceptanceDate).NotEmpty();

        RuleForEach(c => c.Items).ChildRules(items =>
        {
            items.RuleFor(i => i.PurchaseItemId).NotEmpty();
            items.RuleFor(i => i.QtyAccepted).GreaterThan(0);
        });
    }
}

