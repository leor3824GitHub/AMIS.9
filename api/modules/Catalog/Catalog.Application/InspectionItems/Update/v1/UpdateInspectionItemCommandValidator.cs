using FluentValidation;

namespace AMIS.WebApi.Catalog.Application.Inspections.UpdateItem.v1;

public class UpdateInspectionItemCommandValidator : AbstractValidator<UpdateInspectionItemCommand>
{
    public UpdateInspectionItemCommandValidator()
    {
        RuleFor(x => x.QuantityInspected).GreaterThanOrEqualTo(0);
        RuleFor(x => x.QuantityPassed).GreaterThanOrEqualTo(0);
        RuleFor(x => x.QuantityFailed).GreaterThanOrEqualTo(0);
    }
}
