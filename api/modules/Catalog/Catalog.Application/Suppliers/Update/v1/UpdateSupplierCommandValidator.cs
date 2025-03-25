using FluentValidation;

namespace AMIS.WebApi.Catalog.Application.Suppliers.Update.v1;
public class UpdateSupplierCommandValidator : AbstractValidator<UpdateSupplierCommand>
{
    public UpdateSupplierCommandValidator()
    {
        RuleFor(b => b.Name).NotEmpty().MinimumLength(2).MaximumLength(100);
        RuleFor(b => b.Address).MaximumLength(1000);
    }
}
