using System;
using System.Collections.Generic;
using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.ValueObjects;

namespace AMIS.WebApi.Inventories.Application.Services.v1;

/// <summary>
/// Application-layer factory that transforms posted Acceptances into PhysicalAsset instances.
/// Asset classification is determined dynamically from acquisition cost.
/// Delegates property code generation to supplied functions so callers can pull metadata from configuration or other services.
/// </summary>
public static class AcceptanceAssetFactory
{
    public static IReadOnlyCollection<PhysicalAsset> CreateAssets(
        Acceptance acceptance,
        Func<AcceptanceItem, string> propertyCodeFactory)
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

            var asset = PhysicalAsset.Create(
                propertyCode: propertyCode,
                productId: purchaseItem.ProductId.Value,
                acquisitionCost: purchaseItem.UnitPrice * acceptanceItem.QtyAccepted,
                acquisitionDate: acceptance.AcceptanceDate,
                quantity: acceptanceItem.QtyAccepted,
                serialNumber: null,
                modelNumber: null);

            assets.Add(asset);
        }

        return assets;
    }
}

