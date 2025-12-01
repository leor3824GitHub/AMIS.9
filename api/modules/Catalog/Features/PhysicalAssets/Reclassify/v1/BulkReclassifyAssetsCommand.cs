using MediatR;

namespace AMIS.WebApi.Catalog.Features.PhysicalAssets.Reclassify.v1;

/// <summary>
/// Bulk reclassify assets when COA/DBM changes thresholds
/// Example: COA Circular changes PPE threshold from ₱15,000 to ₱50,000
/// </summary>
public record BulkReclassifyAssetsCommand : IRequest<BulkReclassifyAssetsResponse>
{
    public DateTime EffectiveDate { get; init; }
    public string Reason { get; init; } = default!; // e.g., "COA Circular 2022-004 Amendment"
    public string COAReference { get; init; } = default!;
}

public record BulkReclassifyAssetsResponse(
    int TotalProcessed,
    int PPEDowngradedToSemi,
    int SemiUpgradedToPPE,
    List<AssetReclassificationSummary> Summary);

public record AssetReclassificationSummary(
    Guid AssetId,
    string AssetNumber,
    string OldClassification,
    string NewClassification,
    decimal AcquisitionCost);