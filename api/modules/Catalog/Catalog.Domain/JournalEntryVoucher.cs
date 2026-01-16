using System.Linq;
using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;

namespace AMIS.WebApi.Catalog.Domain;

/// <summary>
/// Represents a Journal Entry Voucher (JEV) header for accounting depreciation entries.
/// Contains monthly depreciation entries for multiple PPE assets.
/// Supports export to Excel and PDF formats for accounting records.
/// </summary>
public class JournalEntryVoucher : AuditableEntity, IAggregateRoot
{
    public string VoucherNumber { get; private set; } = default!; // Auto-generated format: JEV-YYYYMM-XXXXX
    public DateTime VoucherDate { get; private set; }
    public int Month { get; private set; } // 1-12
    public int Year { get; private set; }
    public string DepreciationMethod { get; private set; } = "Straight-Line"; // Fixed for now
    public decimal TotalDebitAmount { get; private set; }
    public decimal TotalCreditAmount { get; private set; }
    public JournalEntryVoucherStatus Status { get; private set; }
    public DateTime? PostedDate { get; private set; }
    public DateTime? ExportedDate { get; private set; }
    public string? ExportFormat { get; private set; } // Excel, PDF
    public string? ExportFileName { get; private set; }
    public string? Remarks { get; private set; }

    // Navigation
    public virtual ICollection<JournalEntry> Entries { get; private set; } = [];

    // Computed properties
    public bool HasEntries => Entries.Count > 0;
    public int EntryCount => Entries.Count;
    public bool IsBalanced => Math.Abs(TotalDebitAmount - TotalCreditAmount) < 0.01m; // Allow for rounding
    public DateTime ScheduleDate => new DateTime(Year, Month, 1);

    private JournalEntryVoucher() { }

    private JournalEntryVoucher(
        Guid id,
        string voucherNumber,
        DateTime voucherDate,
        int month,
        int year)
    {
        Id = id;
        VoucherNumber = voucherNumber;
        VoucherDate = voucherDate;
        Month = month;
        Year = year;
        Status = JournalEntryVoucherStatus.Draft;
    }

    /// <summary>
    /// Creates a new JEV for a specific month/year.
    /// Auto-generates voucher number.
    /// </summary>
    public static JournalEntryVoucher Create(int month, int year, DateTime? voucherDate = null)
    {
        if (month < 1 || month > 12)
            throw new ArgumentException("Month must be between 1 and 12.", nameof(month));

        if (year < 2000 || year > DateTime.UtcNow.Year + 10)
            throw new ArgumentException("Year must be valid and reasonable.", nameof(year));

        var date = voucherDate ?? DateTime.UtcNow;
        var voucherNumber = GenerateVoucherNumber(month, year);

        return new JournalEntryVoucher(Guid.NewGuid(), voucherNumber, date, month, year);
    }

    /// <summary>
    /// Adds a journal entry line item to the voucher.
    /// </summary>
    public void AddEntry(JournalEntry entry)
    {
        if (Status != JournalEntryVoucherStatus.Draft)
            throw new InvalidOperationException("Cannot add entries to a posted or exported JEV.");

        if (entry == null)
            throw new ArgumentNullException(nameof(entry));

        if (entry.JournalEntryVoucherId != Id)
            throw new ArgumentException("Entry does not belong to this voucher.", nameof(entry));

        Entries.Add(entry);
        RecalculateTotals();
    }

    /// <summary>
    /// Removes a journal entry line item.
    /// </summary>
    public void RemoveEntry(Guid entryId)
    {
        if (Status != JournalEntryVoucherStatus.Draft)
            throw new InvalidOperationException("Cannot remove entries from a posted or exported JEV.");

        var entry = Entries.FirstOrDefault(e => e.Id == entryId);
        if (entry != null)
        {
            Entries.Remove(entry);
            RecalculateTotals();
        }
    }

    /// <summary>
    /// Posts the JEV to the general ledger.
    /// </summary>
    public void Post()
    {
        if (Status != JournalEntryVoucherStatus.Draft)
            throw new InvalidOperationException($"Cannot post a JEV with status {Status}.");

        if (!HasEntries)
            throw new InvalidOperationException("Cannot post a JEV with no entries.");

        if (!IsBalanced)
            throw new InvalidOperationException("Journal entry is not balanced. Debit and credit amounts must be equal.");

        Status = JournalEntryVoucherStatus.Posted;
        PostedDate = DateTime.UtcNow;
    }

    /// <summary>
    /// Marks the JEV as exported in the specified format.
    /// </summary>
    public void MarkAsExported(string format, string? fileName = null)
    {
        if (string.IsNullOrWhiteSpace(format))
            throw new ArgumentException("Export format must be provided.", nameof(format));

        if (!new[] { "Excel", "PDF", "CSV" }.Contains(format))
            throw new ArgumentException("Invalid export format. Must be Excel, PDF, or CSV.", nameof(format));

        ExportFormat = format;
        ExportFileName = fileName;
        ExportedDate = DateTime.UtcNow;
        Status = JournalEntryVoucherStatus.Exported;
    }

    /// <summary>
    /// Reverses the JEV (creates reverse entries).
    /// </summary>
    public void Reverse(string reason = "Manual reversal")
    {
        if (Status == JournalEntryVoucherStatus.Reversed)
            throw new InvalidOperationException("JEV already reversed.");

        Status = JournalEntryVoucherStatus.Reversed;
        Remarks = reason;
    }

    /// <summary>
    /// Recalculates total debit and credit amounts.
    /// </summary>
    private void RecalculateTotals()
    {
        TotalDebitAmount = Entries.Where(e => e.IsDebit).Sum(e => e.DebitAmount);
        TotalCreditAmount = Entries.Where(e => e.IsCredit).Sum(e => e.CreditAmount);
    }

    /// <summary>
    /// Generates a voucher number in format: JEV-YYYYMM-XXXXX
    /// </summary>
    private static string GenerateVoucherNumber(int month, int year)
    {
        // Note: In a real system, this would include a sequential number from the database
        // For now, using a simple format with timestamp
        var timestamp = DateTime.UtcNow.Ticks % 100000;
        return $"JEV-{year:D4}{month:D2}-{timestamp:D5}";
    }
}
