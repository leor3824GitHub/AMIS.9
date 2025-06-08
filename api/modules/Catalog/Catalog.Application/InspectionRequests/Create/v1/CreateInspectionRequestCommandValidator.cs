using FluentValidation;

namespace AMIS.WebApi.Catalog.Application.InspectionRequests.Create.v1;

public class CreateInspectionRequestCommandValidator : AbstractValidator<CreateInspectionRequestCommand>
{
    public CreateInspectionRequestCommandValidator()
    {
        RuleFor(c => c.PurchaseId).NotEmpty();
        RuleFor(c => c.RequestedById).NotEmpty();
    }
}
