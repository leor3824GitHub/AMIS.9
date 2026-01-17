using MediatR;

namespace AMIS.WebApi.Inventories.Application.PhysicalAssets.Return.v1;

/// <summary>
/// Command to return a physical asset from an employee
/// </summary>
public record ReturnPhysicalAssetCommand : IRequest<ReturnPhysicalAssetResponse>
{
    public Guid Id { get; init; }
    public string Reason { get; init; } = string.Empty;
    public string Condition { get; init; } = string.Empty;
    public Guid AcceptedBy { get; init; }
    public int? QuantityReturned { get; init; }
}

/// <summary>
/// Response for returning a physical asset
/// </summary>
public record ReturnPhysicalAssetResponse
{
    public Guid AssetId { get; init; }
    public Guid AssignmentHistoryId { get; init; }
    public string ReturnNotes { get; init; } = string.Empty;
    public DateTime ReturnedDate { get; init; }
    public string Message { get; init; } = "Asset returned successfully";
}

