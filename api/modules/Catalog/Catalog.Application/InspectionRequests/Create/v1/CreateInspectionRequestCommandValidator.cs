using FluentValidation;

namespace AMIS.WebApi.Catalog.Application.InspectionRequests.Create.v1;

public class CreateInspectionRequestCommandValidator : AbstractValidator<CreateInspectionRequestCommand>
{
    public CreateInspectionRequestCommandValidator()
    {
        RuleFor(c => c.PurchaseId)
           .NotEmpty()
           .WithMessage("PurchaseId is required.");

        RuleFor(c => c.InspectorId)
            .Must(id => id == null || id != Guid.Empty)
            .WithMessage("AssignedInspectorId must be a valid GUID if provided.");
    }
}
