using FluentValidation;

namespace AMIS.WebApi.Catalog.Application.Employees.Update.v1;
public class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeCommandValidator()
    {
        RuleFor(b => b.Name).NotEmpty().MinimumLength(2).MaximumLength(100);
        RuleFor(b => b.Designation).NotEmpty().MaximumLength(100);
    }
}
