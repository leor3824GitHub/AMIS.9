namespace AMIS.WebApi.Inventories.Application.AssetMovement;

/// <summary>
/// Represents a single movement/custody entry for an asset
/// Can originate from PAR, ICS, PPEIR, or SMIR documents
/// </summary>
public record AssetMovementEntryDto
{
    public Guid Id { get; init; }
    public string DocumentNumber { get; init; } = default!;
    public string DocumentType { get; init; } = default!;    // PAR, ICS, PPEIR, SMIR
    public DateTime Date { get; init; }
    public Guid EmployeeId { get; init; }
    public string EmployeeName { get; init; } = default!;
    public string MovementType { get; init; } = default!;    // Issuance, Transfer, Return
    public int Quantity { get; init; } = 1;
    public string? Location { get; init; }
    public string? Condition { get; init; }
    public string? Remarks { get; init; }
    public bool IsActive { get; init; }  // True if this is the current holder
}

/// <summary>
/// Complete asset movement history and current custody information
/// </summary>
public record AssetMovementSummaryDto
{
    public Guid AssetId { get; init; }
    public string PropertyCode { get; init; } = default!;
    public string AssetDescription { get; init; } = default!;
    public decimal AcquisitionCost { get; init; }
    
    // Current custody
    public Guid? CurrentHolderId { get; init; }
    public string? CurrentHolderName { get; init; }
    public string? CurrentStatus { get; init; }  // Active, Returned, Transferred
    public DateTime? CurrentAsOfDate { get; init; }
    
    // Movement history (chronological)
    public List<AssetMovementEntryDto> MovementHistory { get; init; } = new();
}
