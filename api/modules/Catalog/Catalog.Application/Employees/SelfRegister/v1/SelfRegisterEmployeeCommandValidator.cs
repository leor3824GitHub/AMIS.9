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

        RuleFor(x => x.ResponsibilityCode)
            .NotEmpty()
            .MaximumLength(10);

        RuleFor(x => x.Department)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(200);

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .MaximumLength(20);
    }
}
