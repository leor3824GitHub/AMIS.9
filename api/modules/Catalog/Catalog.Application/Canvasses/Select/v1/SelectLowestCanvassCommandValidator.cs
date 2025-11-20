using FluentValidation;

namespace AMIS.WebApi.Catalog.Application.Canvasses.Select.v1;

public sealed class SelectLowestCanvassCommandValidator : AbstractValidator<SelectLowestCanvassCommand>
{
    public SelectLowestCanvassCommandValidator()
    {
        RuleFor(x => x.PurchaseRequestId)
            .NotEmpty();
    }
}
