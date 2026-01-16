namespace AMIS.WebApi.Catalog.Domain;

/// <summary>
/// Enum for Asset Requisition Status
/// Tracks the lifecycle of asset acceptance requests sent to end users
/// </summary>
public enum AssetRequisitionStatus
{
    Pending = 0,        // Waiting for end-user response
    Accepted = 1,       // End-user accepted the asset
    Rejected = 2,       // End-user rejected the asset
    Cancelled = 3,      // Administrator cancelled the request
    Expired = 4         // Request expired without response
}
