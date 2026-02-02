using FluentValidation;
using AMIS.WebApi.Inventories.Domain.ValueObjects;

namespace AMIS.WebApi.Inventories.Application.Maintenance.Schedule.v1;

/// <summary>
/// Validator for ScheduleMaintenanceCommand
/// Ensures required fields are present and valid
/// </summary>
public sealed class ScheduleMaintenanceValidator : AbstractValidator<ScheduleMaintenanceCommand>
{
    public ScheduleMaintenanceValidator()
    {
        RuleFor(x => x.PhysicalAssetId)
            .NotEmpty()
            .WithMessage("Physical Asset ID is required.");

        RuleFor(x => x.MaintenanceType)
            .NotEmpty()
            .WithMessage("Maintenance type is required.")
            .Must(type => ValidMaintenanceTypes.Contains(type, StringComparer.OrdinalIgnoreCase))
            .WithMessage($"Maintenance type must be one of: {string.Join(", ", ValidMaintenanceTypes)}");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.")
            .MaximumLength(500)
            .WithMessage("Description cannot exceed 500 characters.");

        RuleFor(x => x.ScheduledDate)
            .NotEmpty()
            .WithMessage("Scheduled date is required.");

        RuleFor(x => x.EstimatedCost)
            .GreaterThanOrEqualTo(0)
            .When(x => x.EstimatedCost.HasValue)
            .WithMessage("Estimated cost cannot be negative.");

        RuleFor(x => x.CostReference)
            .MaximumLength(100)
            .WithMessage("Cost reference cannot exceed 100 characters.");
    }

    private static readonly string[] ValidMaintenanceTypes = 
        Enum.GetNames(typeof(MaintenanceType));
}
