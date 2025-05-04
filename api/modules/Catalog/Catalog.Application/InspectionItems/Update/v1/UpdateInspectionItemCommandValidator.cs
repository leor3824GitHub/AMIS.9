using FluentValidation;

namespace AMIS.WebApi.Catalog.Application.Inspections.UpdateItem.v1;

public class UpdateInspectionItemCommandValidator : AbstractValidator<UpdateInspectionItemCommand>
{
    public UpdateInspectionItemCommandValidator()
    {
        RuleFor(x => x.QtyInspected).GreaterThanOrEqualTo(0);
        RuleFor(x => x.QtyPassed).GreaterThanOrEqualTo(0);
        RuleFor(x => x.QtyFailed).GreaterThanOrEqualTo(0);
    }
}
