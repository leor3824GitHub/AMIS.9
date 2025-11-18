using FluentValidation;

namespace AMIS.WebApi.Catalog.Application.Employees.SelfRegister.v1;

public class SelfRegisterEmployeeCommandValidator : AbstractValidator<SelfRegisterEmployeeCommand>
{
    public SelfRegisterEmployeeCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(100);

        RuleFor(x => x.Designation)
            .NotEmpty()
            .MaximumLength(100);
    }
}
