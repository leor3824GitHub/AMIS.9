using FluentValidation;

namespace AMIS.WebApi.Inventories.Application.Disposals.Request.v1;

/// <summary>
/// Validator for CreateDisposalRequestCommand
/// Ensures required fields are present and valid
/// </summary>
public sealed class CreateDisposalRequestValidator : AbstractValidator<CreateDisposalRequestCommand>
{
    public CreateDisposalRequestValidator()
    {
        RuleFor(x => x.PhysicalAssetId)
            .NotEmpty()
            .WithMessage("Physical Asset ID is required.");

        RuleFor(x => x.DisposalMethod)
            .NotEmpty()
            .WithMessage("Disposal method is required.")
            .Must(method => ValidDisposalMethods.Contains(method, StringComparer.OrdinalIgnoreCase))
            .WithMessage($"Disposal method must be one of: {string.Join(", ", ValidDisposalMethods)}");

        RuleFor(x => x.AssetConditionAtDisposal)
            .NotEmpty()
            .WithMessage("Asset condition is required.")
            .Must(condition => ValidConditions.Contains(condition, StringComparer.OrdinalIgnoreCase))
            .WithMessage($"Asset condition must be one of: {string.Join(", ", ValidConditions)}");

        RuleFor(x => x.JustificationReason)
            .MaximumLength(500)
            .WithMessage("Justification reason cannot exceed 500 characters.");
    }

    private static readonly string[] ValidDisposalMethods = ["Sale", "Scrap", "Donation", "Transfer", "Condemnation"];
    private static readonly string[] ValidConditions = ["Good", "Fair", "Poor", "Unserviceable", "ForDisposal"];
}
