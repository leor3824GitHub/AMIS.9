using AMIS.WebApi.Catalog.Domain.ValueObjects;
using FluentValidation;

namespace AMIS.WebApi.Catalog.Application.InspectionRequests.Update.v1;

public class UpdateInspectionRequestCommandValidator : AbstractValidator<UpdateInspectionRequestCommand>
{
    public UpdateInspectionRequestCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.PurchaseId).NotEmpty();
        RuleFor(x => x.RequestedById).NotEmpty();

        // Validate AssignedInspectorId only if provided
        RuleFor(x => x.AssignedInspectorId)
            .Must(id => id == null || id != Guid.Empty)
            .WithMessage("AssignedInspectorId must be a valid GUID if provided.");

        // If AssignedInspectorId is provided, status must be Assigned or Completed
        RuleFor(x => x)
            .Must(x => x.AssignedInspectorId == null || x.Status == InspectionRequestStatus.Assigned || x.Status == InspectionRequestStatus.Completed)
            .WithMessage("If an inspector is assigned, the status must be Assigned or Completed.");

        // If status is Completed, AssignedInspectorId must not be null
        RuleFor(x => x)
            .Must(x => x.Status != InspectionRequestStatus.Completed || x.AssignedInspectorId != null)
            .WithMessage("Completed inspections must have an assigned inspector.");
    }
}
