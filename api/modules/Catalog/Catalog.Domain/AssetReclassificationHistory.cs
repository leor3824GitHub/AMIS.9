using AMIS.Framework.Core.Domain;
using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Domain;

/// <summary>
/// Tracks reclassification history when COA/DBM changes asset thresholds
/// Example: Asset reclassified from PPE to Semi-Expendable when threshold changed from ₱15,000 to ₱50,000
/// Part of PhysicalAsset aggregate - not a standalone aggregate root
/// </summary>
public class AssetReclassificationHistory : AuditableEntity
{
    public Guid AssetId { get; private set; }
    public string AssetNumber { get; private set; } = default!;
    public PropertyClassification OldClassification { get; private set; }
    public PropertyClassification NewClassification { get; private set; }
    public DateTime EffectiveDate { get; private set; }
    public string Reason { get; private set; } = default!;
    public decimal AcquisitionCostAtReclassification { get; private set; }
    public string? COAReference { get; private set; } // e.g., "COA Circular 2025-001"
    public string? Remarks { get; private set; }

    // Navigation properties
    public virtual PhysicalAsset Asset { get; private set; } = default!;

    private AssetReclassificationHistory() { }

    private AssetReclassificationHistory(
        Guid id,
        Guid assetId,
        string assetNumber,
        PropertyClassification oldClassification,
        PropertyClassification newClassification,
        DateTime effectiveDate,
        string reason,
        decimal acquisitionCost)
    {
        Id = id;
        AssetId = assetId;
        AssetNumber = assetNumber;
        OldClassification = oldClassification;
        NewClassification = newClassification;
        EffectiveDate = effectiveDate;
        Reason = reason;
        AcquisitionCostAtReclassification = acquisitionCost;
    }

    public static AssetReclassificationHistory Create(
        Guid assetId,
        string assetNumber,
        PropertyClassification oldClassification,
        PropertyClassification newClassification,
        DateTime effectiveDate,
        string reason,
        decimal acquisitionCost,
        string? coaReference = null)
    {
        var history = new AssetReclassificationHistory(
            Guid.NewGuid(),
            assetId,
            assetNumber,
            oldClassification,
            newClassification,
            effectiveDate,
            reason,
            acquisitionCost);

        history.COAReference = coaReference;

        return history;
    }

    public void AddRemarks(string remarks)
    {
        if (string.IsNullOrWhiteSpace(remarks))
            throw new ArgumentException("Remarks cannot be empty.");

        Remarks = string.IsNullOrWhiteSpace(Remarks)
            ? remarks
            : $"{Remarks}\n{DateTime.UtcNow:yyyy-MM-dd}: {remarks}";
    }

    /// <summary>
    /// Determine if this was an upgrade (Semi to PPE) or downgrade (PPE to Semi)
    /// </summary>
    public string GetReclassificationType()
    {
        if (OldClassification == PropertyClassification.SemiExpendable &&
            NewClassification == PropertyClassification.PropertyPlantEquipment)
            return "Upgrade";

        if (OldClassification == PropertyClassification.PropertyPlantEquipment &&
            NewClassification == PropertyClassification.SemiExpendable)
            return "Downgrade";

        return "Lateral";
    }
}
