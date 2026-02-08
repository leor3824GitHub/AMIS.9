using System;
using System.Collections.Generic;
using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.ValueObjects;

namespace AMIS.WebApi.Inventories.Application.Acceptances.Services;

/// <summary>
/// Application-layer factory that turns posted Acceptances into PhysicalAsset instances.
/// Asset classification is determined dynamically from acquisition cost.
/// Delegates property code generation to caller-provided resolvers so infrastructure/config lookups stay outside the domain.
/// </summary>
public static class AcceptanceAssetFactory
{
    public static async Task<IReadOnlyCollection<PhysicalAsset>> CreateAssetsAsync(
        Acceptance acceptance,
        IAssetPropertyCodeGenerator codeGenerator,
        IAssetClassificationResolver classificationResolver,
        CancellationToken cancellationToken = default)
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

            var classification = classificationResolver.ResolveClassification(purchaseItem);

            if (classification == PropertyClassification.PropertyPlantEquipment)
            {
                for (var i = 0; i < acceptanceItem.QtyAccepted; i++)
                {
                    var propertyCode = await codeGenerator.GenerateAsync(acceptanceItem, cancellationToken).ConfigureAwait(false);
                    var asset = PhysicalAsset.Create(
                        propertyCode,
                        purchaseItem.ProductId.Value,
                        purchaseItem.UnitPrice,
                        acceptance.AcceptanceDate,
                        quantity: 1,
                        serialNumber: null,
                        modelNumber: null);

                    assets.Add(asset);
                }
            }
            else
            {
                var propertyCode = await codeGenerator.GenerateAsync(acceptanceItem, cancellationToken).ConfigureAwait(false);
                var asset = PhysicalAsset.Create(
                    propertyCode,
                    purchaseItem.ProductId.Value,
                    purchaseItem.UnitPrice * acceptanceItem.QtyAccepted,
                    acceptance.AcceptanceDate,
                    acceptanceItem.QtyAccepted,
                    serialNumber: null,
                    modelNumber: null);

                assets.Add(asset);
            }
        }

        return assets;
    }
}

