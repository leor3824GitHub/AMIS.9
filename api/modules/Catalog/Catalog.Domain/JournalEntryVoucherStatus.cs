namespace AMIS.WebApi.Catalog.Domain;

/// <summary>
/// Enum for Journal Entry Voucher Status
/// </summary>
public enum JournalEntryVoucherStatus
{
    Draft = 0,          // Being prepared
    Pending = 1,        // Submitted for approval
    Posted = 2,         // Posted to general ledger
    Exported = 3,       // Exported to Excel/PDF
    Reversed = 4,       // JEV reversed
    Rejected = 5        // Approval rejected
}
