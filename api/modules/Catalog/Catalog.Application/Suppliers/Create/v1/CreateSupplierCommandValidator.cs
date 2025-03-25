using FluentValidation;

namespace AMIS.WebApi.Catalog.Application.Suppliers.Create.v1;
public class CreateSupplierCommandValidator : AbstractValidator<CreateSupplierCommand>
{
    public CreateSupplierCommandValidator()
    {
        RuleFor(b => b.Name).NotEmpty().MinimumLength(2).MaximumLength(100);
        RuleFor(b => b.Address).MaximumLength(1000);
    }
}
