using FluentValidation;

namespace AMIS.WebApi.Catalog.Application.Issuances.Update.v1;
public class UpdateIssuanceCommandValidator : AbstractValidator<UpdateIssuanceCommand>
{
    public UpdateIssuanceCommandValidator()
    {
        RuleFor(p => p.ProductId).NotEmpty();
        RuleFor(p => p.EmployeeId).NotEmpty();
    }
}
