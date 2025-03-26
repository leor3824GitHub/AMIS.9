using FluentValidation;

namespace AMIS.WebApi.Catalog.Application.IssuanceItems.Create.v1;
public class CreateIssuanceItemCommandValidator : AbstractValidator<CreateIssuanceItemCommand>
{
    public CreateIssuanceItemCommandValidator()
    {
        RuleFor(p => p.IssuanceId).NotEmpty();
        RuleFor(p => p.ProductId).NotEmpty();
    }
}
