using MediatR;

namespace AMIS.WebApi.Inventories.Application.Services;

/// <summary>
/// Command to reclassify assets when COA/DBM threshold changes occur
/// Example: When PPE threshold changes from ₱50,000 to ₱100,000,
/// assets between ₱50,001-₱100,000 must be reclassified from PPE to Semi-Expendable
/// </summary>
public sealed record ReclassifyAssetsCommand(
    decimal NewPPEThreshold,
    DateTime EffectiveDate,
    string COAReference,
    string Reason) : IRequest<ReclassifyAssetsResponse>;

public sealed record ReclassifyAssetsResponse(
    int AssetsReclassified,
    decimal OldThreshold,
    decimal NewThreshold,
    IReadOnlyList<AssetReclassificationDetail> ReclassificationDetails);

public sealed record AssetReclassificationDetail(
    Guid AssetId,
    string PropertyCode,
    string Description,
    decimal AcquisitionCost,
    string OldClassification,
    string NewClassification);
