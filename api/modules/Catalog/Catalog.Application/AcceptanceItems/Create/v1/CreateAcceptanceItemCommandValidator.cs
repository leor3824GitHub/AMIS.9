using FluentValidation;

namespace AMIS.WebApi.Catalog.Application.AcceptanceItems.Create.v1;

public class CreateAcceptanceItemCommandValidator : AbstractValidator<CreateAcceptanceItemCommand>
{
    public CreateAcceptanceItemCommandValidator()
    {
        RuleFor(x => x.AcceptanceId).NotEmpty();
        RuleFor(x => x.PurchaseItemId).NotEmpty();
        RuleFor(x => x.QtyAccepted).GreaterThan(0);       
    }
}
