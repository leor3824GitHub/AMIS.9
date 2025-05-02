using FluentValidation;

namespace AMIS.WebApi.Catalog.Application.Inspections.Update.v1;

public class UpdateInspectionCommandValidator : AbstractValidator<UpdateInspectionCommand>
{
    public UpdateInspectionCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.InspectionDate).NotEmpty();
        RuleFor(x => x.InspectedBy).NotEmpty();
    }
}
