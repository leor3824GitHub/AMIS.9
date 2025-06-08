using FluentValidation;

namespace AMIS.WebApi.Catalog.Application.InspectionRequests.Update.v1;

public class UpdateInspectionRequestCommandValidator : AbstractValidator<UpdateInspectionRequestCommand>
{
    public UpdateInspectionRequestCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.PurchaseId).NotEmpty();
        RuleFor(x => x.RequestedById).NotEmpty();
    }
}
