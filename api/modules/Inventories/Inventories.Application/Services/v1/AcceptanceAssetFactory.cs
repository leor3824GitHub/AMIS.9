using System;
using System.Collections.Generic;
using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.ValueObjects;

namespace AMIS.WebApi.Inventories.Application.Services.v1;

/// <summary>
/// Application-layer factory that transforms posted Acceptances into PhysicalAsset instances.
/// Delegates property code generation and classification lookups to supplied functions so callers can pull metadata from configuration or other services.
/// </summary>
public static class AcceptanceAssetFactory
{
    public static IReadOnlyCollection<PhysicalAsset> CreateAssets(
        Acceptance acceptance,
        Func<AcceptanceItem, string> propertyCodeFactory,
        Func<PurchaseItem, PropertyClassification> classificationResolver,
        Func<PurchaseItem, string?> ppeTypeResolver,
        Func<PurchaseItem, int> estimatedUsefulLifeResolver,
        Func<PurchaseItem, string> descriptionResolver,
        Func<PurchaseItem, string> unitOfMeasureResolver)
    {
        ArgumentNullException.ThrowIfNull(acceptance);
        if (!acceptance.IsPosted) throw new InvalidOperationException("Acceptance must be posted before creating assets.");
        if (acceptance.Items.Count == 0) throw new InvalidOperationException("Acceptance has no items to convert to assets.");

        var assets = new List<PhysicalAsset>();

        foreach (var acceptanceItem in acceptance.Items)
        {
            var purchaseItem = acceptanceItem.PurchaseItem;
            if (purchaseItem is null)
                throw new InvalidOperationException("Acceptance item is missing its PurchaseItem navigation.");

            if (!purchaseItem.ProductId.HasValue)
                throw new InvalidOperationException("Purchase item must have an associated product to create an asset.");

            var propertyCode = propertyCodeFactory(acceptanceItem);
            var classification = classificationResolver(purchaseItem);
            var ppeType = ppeTypeResolver(purchaseItem);
            var estimatedUsefulLife = estimatedUsefulLifeResolver(purchaseItem);
            var description = descriptionResolver(purchaseItem);
            var unitOfMeasure = unitOfMeasureResolver(purchaseItem);

            var asset = PhysicalAsset.Create(
                classification,
                propertyCode,
                purchaseItem.ProductId.Value,
                description,
                purchaseItem.UnitPrice * acceptanceItem.QtyAccepted,
                acceptance.AcceptanceDate,
                estimatedUsefulLife,
                acceptanceItem.QtyAccepted,
                unitOfMeasure,
                serialNumber: null,
                modelNumber: null,
                location: null,
                ppeType: ppeType);

            assets.Add(asset);
        }

        return assets;
    }
}

