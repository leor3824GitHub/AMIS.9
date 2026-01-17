using FluentValidation;

namespace AMIS.WebApi.Inventories.Application.Issuances.Update.v1;
public class UpdateIssuanceCommandValidator : AbstractValidator<UpdateIssuanceCommand>
{
    public UpdateIssuanceCommandValidator()
    {
        RuleFor(p => p.EmployeeId).NotEmpty();
    }
}

