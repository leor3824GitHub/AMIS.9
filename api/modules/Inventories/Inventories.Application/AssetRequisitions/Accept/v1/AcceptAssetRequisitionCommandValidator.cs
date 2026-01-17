using FluentValidation;

namespace AMIS.WebApi.Inventories.Application.AssetRequisitions.Accept.v1;

public class AcceptAssetRequisitionCommandValidator : AbstractValidator<AcceptAssetRequisitionCommand>
{
    public AcceptAssetRequisitionCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Asset requisition ID is required");

        RuleFor(x => x.SignatureData)
            .NotEmpty()
            .WithMessage("Signature data is required");

        RuleFor(x => x.IpAddress)
            .NotEmpty()
            .WithMessage("IP address is required");

        RuleFor(x => x.UserAgent)
            .NotEmpty()
            .WithMessage("User agent is required");
    }
}

