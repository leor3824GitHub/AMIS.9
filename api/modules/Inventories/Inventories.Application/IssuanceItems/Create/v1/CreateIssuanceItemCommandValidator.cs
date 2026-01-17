using FluentValidation;

namespace AMIS.WebApi.Inventories.Application.IssuanceItems.Create.v1;
public class CreateIssuanceItemCommandValidator : AbstractValidator<CreateIssuanceItemCommand>
{
    public CreateIssuanceItemCommandValidator()
    {
        RuleFor(p => p.IssuanceId).NotEmpty();
        RuleFor(p => p.ProductId).NotEmpty();
    }
}

