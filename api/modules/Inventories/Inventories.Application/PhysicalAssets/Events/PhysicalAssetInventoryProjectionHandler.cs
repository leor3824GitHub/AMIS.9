using System;
using System.Threading;
using System.Threading.Tasks;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.Events;
using AMIS.WebApi.Inventories.Application.InventoryRegistries.Specs;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Inventories.Application.PhysicalAssets.Events;

public sealed class PhysicalAssetInventoryProjectionHandler :
    INotificationHandler<PhysicalAssetCreated>,
    INotificationHandler<PhysicalAssetIssued>,
    INotificationHandler<PhysicalAssetReturned>
{
    private readonly IRepository<InventoryRegistry> _registryRepo;
    private readonly ILogger<PhysicalAssetInventoryProjectionHandler> _logger;

    public PhysicalAssetInventoryProjectionHandler(
        [FromKeyedServices("inventories:inventory-registries")] IRepository<InventoryRegistry> registryRepo,
        ILogger<PhysicalAssetInventoryProjectionHandler> logger)
    {
        _registryRepo = registryRepo ?? throw new ArgumentNullException(nameof(registryRepo));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(PhysicalAssetCreated notification, CancellationToken cancellationToken)
    {
        var asset = notification.PhysicalAsset;
        if (asset is null)
        {
            _logger.LogWarning("PhysicalAssetCreated received with null asset");
            return;
        }

        // Normalize property code
        var code = asset.PropertyCode?.Trim().ToUpperInvariant() ?? string.Empty;

        try
        {
            // Try to find existing registry entry
            var existing = await _registryRepo.GetBySpecAsync(new InventoryRegistryByPropertyCodeSpec(code), cancellationToken);
            if (existing == null)
            {
                var registry = InventoryRegistry.CreateFromReceiving(
                    code,
                    asset.Product?.Name ?? string.Empty,
                    asset.Quantity,
                    asset.CurrentAssignment?.Location ?? string.Empty,
                    reportNumber: "AUTO-CREATE-ON-ASSET-CREATE");

                await _registryRepo.AddAsync(registry, cancellationToken);
                _logger.LogInformation("Created InventoryRegistry for PropertyCode {PropertyCode} from asset {AssetId}", code, asset.Id);
            }
            else
            {
                // If registry exists, ensure quantity alignment (add asset.Quantity)
                existing.AddQuantity(asset.Quantity, "AUTO-ADJUST-ASSET-CREATE");
                await _registryRepo.UpdateAsync(existing, cancellationToken);
                _logger.LogInformation("Updated InventoryRegistry {PropertyCode} quantity by {Qty} from asset {AssetId}", code, asset.Quantity, asset.Id);
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed projecting PhysicalAssetCreated for PropertyCode {PropertyCode}", code);
        }
    }

    public async Task Handle(PhysicalAssetIssued notification, CancellationToken cancellationToken)
    {
        var asset = notification.PhysicalAsset;
        if (asset is null)
        {
            _logger.LogWarning("PhysicalAssetIssued received with null asset");
            return;
        }

        var code = asset.PropertyCode?.Trim().ToUpperInvariant() ?? string.Empty;

        try
        {
            var registry = await _registryRepo.GetBySpecAsync(new InventoryRegistryByPropertyCodeSpec(code), cancellationToken);
            if (registry == null)
            {
                _logger.LogWarning("InventoryRegistry not found for PropertyCode {PropertyCode} when issuing asset {AssetId}", code, asset.Id);
                return;
            }

            registry.DeductQuantity(notification.Quantity, notification.DocumentNumber);
            await _registryRepo.UpdateAsync(registry, cancellationToken);
            _logger.LogInformation("Deducted {Qty} from InventoryRegistry {PropertyCode} for asset {AssetId}", notification.Quantity, code, asset.Id);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed projecting PhysicalAssetIssued for PropertyCode {PropertyCode}", code);
        }
    }

    public async Task Handle(PhysicalAssetReturned notification, CancellationToken cancellationToken)
    {
        var asset = notification.PhysicalAsset;
        if (asset is null)
        {
            _logger.LogWarning("PhysicalAssetReturned received with null asset");
            return;
        }

        var code = asset.PropertyCode?.Trim().ToUpperInvariant() ?? string.Empty;

        try
        {
            var registry = await _registryRepo.GetBySpecAsync(new InventoryRegistryByPropertyCodeSpec(code), cancellationToken);
            if (registry == null)
            {
                _logger.LogWarning("InventoryRegistry not found for PropertyCode {PropertyCode} when returning asset {AssetId}", code, asset.Id);
                return;
            }

            registry.AddQuantity(notification.QuantityReturned, reportNumber: notification.Reason ?? "RETURN");
            await _registryRepo.UpdateAsync(registry, cancellationToken);
            _logger.LogInformation("Added {Qty} to InventoryRegistry {PropertyCode} for returned asset {AssetId}", notification.QuantityReturned, code, asset.Id);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed projecting PhysicalAssetReturned for PropertyCode {PropertyCode}", code);
        }
    }
}
