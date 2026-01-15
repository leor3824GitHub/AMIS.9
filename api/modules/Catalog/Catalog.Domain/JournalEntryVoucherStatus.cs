namespace AMIS.WebApi.Catalog.Domain;

/// <summary>
/// Enum for Journal Entry Voucher Status
/// </summary>
public enum JournalEntryVoucherStatus
{
    Draft = 0,          // Being prepared
    Posted = 1,         // Posted to general ledger
    Exported = 2,       // Exported to Excel/PDF
    Reversed = 3        // JEV reversed
}
