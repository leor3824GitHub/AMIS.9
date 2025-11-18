using FluentValidation;

namespace AMIS.WebApi.Catalog.Application.Employees.Create.v1;
public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeCommandValidator()
    {
        RuleFor(b => b.Name).NotEmpty().MinimumLength(2).MaximumLength(100);
        RuleFor(b => b.Designation).NotEmpty().MaximumLength(100);
    }
}
