using FluentValidation;

namespace AMIS.WebApi.Inventories.Application.Canvasses.Select.v1;

public sealed class SelectLowestCanvassCommandValidator : AbstractValidator<SelectLowestCanvassCommand>
{
    public SelectLowestCanvassCommandValidator()
    {
        RuleFor(x => x.PurchaseRequestId)
            .NotEmpty();
    }
}

