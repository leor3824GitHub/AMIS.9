namespace AMIS.WebApi.Catalog.Domain.Services;

/// <summary>
/// Base interface for COA-compliant document generation
/// </summary>
public interface IDocumentGenerationService
{
    Task<byte[]> GeneratePdfAsync<TDocument>(TDocument document, CancellationToken cancellationToken = default)
        where TDocument : class;
}

/// <summary>
/// RSMI (Requisition and Issue Slip) document data model
/// For consumable items
/// </summary>
public record RSMIDocument
{
    public string RSMINumber { get; init; } = default!;
    public DateTime IssueDate { get; init; }
    public string RequestedBy { get; init; } = default!;
    public string ApprovedBy { get; init; } = default!;
    public string IssuedBy { get; init; } = default!;
    public string ReceivedBy { get; init; } = default!;
    public List<RSMILineItem> Items { get; init; } = new();
}

public record RSMILineItem
{
    public string StockNumber { get; init; } = default!;
    public string Description { get; init; } = default!;
    public string UnitOfMeasure { get; init; } = default!;
    public int Quantity { get; init; }
    public decimal UnitCost { get; init; }
    public decimal TotalCost => Quantity * UnitCost;
}

/// <summary>
/// ICS (Inventory Custodian Slip) document data model
/// For semi-expendable items (₱1,000 - ₱50,000)
/// </summary>
public record ICSDocument
{
    public string ICSNumber { get; init; } = default!;
    public DateTime IssueDate { get; init; }
    public string CustodianName { get; init; } = default!;
    public string CustodianPosition { get; init; } = default!;
    public string Department { get; init; } = default!;
    public string IssuedBy { get; init; } = default!;
    public string ReceivedBy { get; init; } = default!;
    public List<ICSLineItem> Items { get; init; } = new();
}

public record ICSLineItem
{
    public string PropertyCode { get; init; } = default!;
    public string Description { get; init; } = default!;
    public string UnitOfMeasure { get; init; } = default!;
    public int Quantity { get; init; }
    public decimal UnitCost { get; init; }
    public decimal TotalCost => Quantity * UnitCost;
    public int EstimatedUsefulLifeMonths { get; init; }
}

/// <summary>
/// PAR (Property Acknowledgment Receipt) document data model
/// For PPE items (> ₱50,000)
/// </summary>
public record PARDocument
{
    public string PARNumber { get; init; } = default!;
    public DateTime IssueDate { get; init; }
    public string CustodianName { get; init; } = default!;
    public string CustodianPosition { get; init; } = default!;
    public string Department { get; init; } = default!;
    public string IssuedBy { get; init; } = default!;
    public string ReceivedBy { get; init; } = default!;
    public List<PARLineItem> Items { get; init; } = new();
}

public record PARLineItem
{
    public string PropertyNumber { get; init; } = default!;
    public string Description { get; init; } = default!;
    public string UnitOfMeasure { get; init; } = default!;
    public int Quantity { get; init; }
    public decimal AcquisitionCost { get; init; }
    public string PPEAccountCode { get; init; } = default!;
    public int EstimatedUsefulLifeYears { get; init; }
}

/// <summary>
/// Stock Ledger Card (SLC) document data model
/// For tracking inventory movements
/// </summary>
public record StockLedgerCard
{
    public string StockNumber { get; init; } = default!;
    public string Description { get; init; } = default!;
    public string UnitOfMeasure { get; init; } = default!;
    public int ReorderLevel { get; init; }
    public List<StockLedgerEntry> Entries { get; init; } = new();
}

public record StockLedgerEntry
{
    public DateTime Date { get; init; }
    public string ReferenceNumber { get; init; } = default!;
    public int QuantityIn { get; init; }
    public int QuantityOut { get; init; }
    public int Balance { get; init; }
    public decimal UnitCost { get; init; }
    public decimal TotalCost { get; init; }
    public string Remarks { get; init; } = default!;
}
