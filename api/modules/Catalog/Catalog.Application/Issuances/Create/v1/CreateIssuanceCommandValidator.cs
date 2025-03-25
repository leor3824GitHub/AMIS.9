using FluentValidation;

namespace AMIS.WebApi.Catalog.Application.Issuances.Create.v1;
public class CreateIssuanceCommandValidator : AbstractValidator<CreateIssuanceCommand>
{
    public CreateIssuanceCommandValidator()
    {
        RuleFor(p => p.ProductId).NotEmpty();
        RuleFor(p => p.EmployeeId).NotEmpty();
    }
}
