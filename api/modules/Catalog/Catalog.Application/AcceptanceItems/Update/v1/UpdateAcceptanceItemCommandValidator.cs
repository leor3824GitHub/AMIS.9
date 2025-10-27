using FluentValidation;

namespace AMIS.WebApi.Catalog.Application.AcceptanceItems.Update.v1;

public class UpdateAcceptanceItemCommandValidator : AbstractValidator<UpdateAcceptanceItemCommand>
{
    public UpdateAcceptanceItemCommandValidator()
    {
        RuleFor(x => x.QtyAccepted).GreaterThanOrEqualTo(0);
    }
}
