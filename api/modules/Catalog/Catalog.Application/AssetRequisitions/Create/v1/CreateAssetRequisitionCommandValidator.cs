using FluentValidation;

namespace AMIS.WebApi.Catalog.Application.AssetRequisitions.Create.v1;

public class CreateAssetRequisitionCommandValidator : AbstractValidator<CreateAssetRequisitionCommand>
{
    public CreateAssetRequisitionCommandValidator()
    {
        RuleFor(x => x.EmployeeId)
            .NotEmpty()
            .WithMessage("Employee ID is required");

        RuleFor(x => x.IssuanceId)
            .NotEmpty()
            .WithMessage("Issuance ID is required");
    }
}
