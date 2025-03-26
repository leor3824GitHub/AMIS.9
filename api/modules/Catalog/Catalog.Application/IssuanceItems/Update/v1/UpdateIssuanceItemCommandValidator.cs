using FluentValidation;

namespace AMIS.WebApi.Catalog.Application.IssuanceItems.Update.v1;
public class UpdateIssuanceItemCommandValidator : AbstractValidator<UpdateIssuanceItemCommand>
{
    public UpdateIssuanceItemCommandValidator()
    {
        RuleFor(p => p.IssuanceId).NotEmpty();
        RuleFor(p => p.ProductId).NotEmpty();
    }
}
