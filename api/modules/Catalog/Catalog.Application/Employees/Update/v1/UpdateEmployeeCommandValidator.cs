using FluentValidation;

namespace AMIS.WebApi.Catalog.Application.Employees.Update.v1;
public class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeCommandValidator()
    {
        RuleFor(b => b.Name).NotEmpty().MinimumLength(2).MaximumLength(100);
        RuleFor(b => b.Designation).NotEmpty().MaximumLength(100);
        RuleFor(b => b.ResponsibilityCode).NotEmpty().MaximumLength(10);
        RuleFor(b => b.Department).NotEmpty().MaximumLength(100);
        RuleFor(b => b.Email).NotEmpty().EmailAddress().MaximumLength(200);
        RuleFor(b => b.PhoneNumber).NotEmpty().MaximumLength(20);
    }
}
