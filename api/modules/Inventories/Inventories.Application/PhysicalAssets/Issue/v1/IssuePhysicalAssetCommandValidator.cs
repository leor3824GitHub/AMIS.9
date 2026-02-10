using AMIS.Framework.Core.Persistence;
using FluentValidation;
using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.ValueObjects;
using AMIS.WebApi.Inventories.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Inventories.Application.PhysicalAssets.Issue.v1;

/// <summary>
/// Validator for issuing a physical asset command
/// </summary>
public sealed class IssuePhysicalAssetCommandValidator : AbstractValidator<IssuePhysicalAssetCommand>
{
    public IssuePhysicalAssetCommandValidator([FromKeyedServices("inventories:physicalassets")] IRepository<PhysicalAsset> repository)
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Asset ID is required.");

        RuleFor(x => x.EmployeeId)
            .NotEmpty().WithMessage("Employee ID is required.");

        RuleFor(x => x.EmployeeName)
            .NotEmpty().WithMessage("Employee name is required.");

        RuleFor(x => x.DocumentNumber)
            .NotEmpty().WithMessage("Document number is required.")
            .MaximumLength(50).WithMessage("Document number cannot exceed 50 characters.");

        RuleFor(x => x)
            .MustAsync(async (command, cancellationToken) => 
            {
                var asset = await repository.GetByIdAsync(command.Id, cancellationToken);
                return asset != null;
            })
            .WithName("AssetId")
            .WithMessage("Physical asset not found.");

        RuleFor(x => x)
            .MustAsync(async (command, cancellationToken) => 
            {
                var asset = await repository.GetByIdAsync(command.Id, cancellationToken);
                if (asset == null) return true; // Let the previous rule handle this
                
                // Check if asset is already assigned
                return asset.CurrentAssignment == null;
            })
            .WithName("Asset")
            .WithMessage("Asset is already assigned to another employee. Use Transfer instead.");

        RuleFor(x => x)
            .MustAsync(async (command, cancellationToken) => 
            {
                var asset = await repository.GetByIdAsync(command.Id, cancellationToken);
                if (asset == null) return true; // Let the previous rule handle this
                
                // Check if asset is disposed
                return !asset.IsDisposed;
            })
            .WithName("Asset")
            .WithMessage("Cannot issue a disposed asset.");

        // Validate quantity for semi-expendable items
        RuleFor(x => x)
            .MustAsync(async (command, cancellationToken) => 
            {
                var asset = await repository.GetByIdAsync(command.Id, cancellationToken);
                if (asset == null) return true; // Let the previous rule handle this
                
                // Check if asset is semi-expendable using current classification
                if (asset.CurrentClassification == PropertyClassification.SemiExpendable)
                {
                    if (!command.QuantityIssued.HasValue || command.QuantityIssued <= 0)
                        return false;
                    if (command.QuantityIssued > asset.Quantity)
                        return false;
                }
                return true;
            })
            .WithName("QuantityIssued")
            .WithMessage("For semi-expendable items, quantity issued must be between 1 and the available quantity.");
    }
}
