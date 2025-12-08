using System;
using System.Collections.Generic;
using AMIS.WebApi.Catalog.Domain;
using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Application.Acceptances.Services;

/// <summary>
/// Application-layer factory that turns posted Acceptances into PhysicalAsset instances.
/// Delegates property code and classification decisions to caller-provided resolvers so infrastructure/config lookups stay outside the domain.
/// </summary>
public static class AcceptanceAssetFactory
{
    public static IReadOnlyCollection<PhysicalAsset> CreateAssets(
        Acceptance acceptance,
        IAssetPropertyCodeGenerator codeGenerator,
        IAssetClassificationResolver classificationResolver)
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

            var propertyCode = codeGenerator.Generate(acceptanceItem);
            var classification = classificationResolver.ResolveClassification(purchaseItem);
            var ppeType = classificationResolver.ResolvePpeType(purchaseItem);
            var estimatedUsefulLife = classificationResolver.ResolveEstimatedUsefulLifeMonths(purchaseItem);
            var description = classificationResolver.ResolveDescription(purchaseItem);
            var unitOfMeasure = classificationResolver.ResolveUnitOfMeasure(purchaseItem);

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
