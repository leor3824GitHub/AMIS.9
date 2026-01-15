using AMIS.Framework.Core.Domain;

namespace AMIS.WebApi.Catalog.Domain;

/// <summary>
/// Represents a single line item in a Journal Entry Voucher.
/// Contains debit/credit information for a specific account code.
/// </summary>
public class JournalEntry : AuditableEntity
{
    public Guid JournalEntryVoucherId { get; private set; }
    public int LineNumber { get; private set; }
    public string AccountCode { get; private set; } = default!;
    public string? AccountName { get; private set; }
    public decimal DebitAmount { get; private set; }
    public decimal CreditAmount { get; private set; }
    public string? Description { get; private set; }

    // Navigation
    public virtual JournalEntryVoucher JournalEntryVoucher { get; private set; } = default!;

    private JournalEntry() { }

    private JournalEntry(
        Guid id,
        Guid voucherId,
        int lineNumber,
        string accountCode,
        string? accountName,
        decimal debitAmount,
        decimal creditAmount,
        string? description)
    {
        Id = id;
        JournalEntryVoucherId = voucherId;
        LineNumber = lineNumber;
        AccountCode = accountCode;
        AccountName = accountName;
        DebitAmount = debitAmount;
        CreditAmount = creditAmount;
        Description = description;
    }

    /// <summary>
    /// Creates a new journal entry line item.
    /// Either debit or credit amount should be provided, not both.
    /// </summary>
    public static JournalEntry Create(
        Guid voucherId,
        int lineNumber,
        string accountCode,
        string? accountName,
        decimal debitAmount,
        decimal creditAmount,
        string? description = null)
    {
        if (voucherId == Guid.Empty)
            throw new ArgumentException("Voucher ID must be provided.", nameof(voucherId));

        if (lineNumber < 1)
            throw new ArgumentException("Line number must be greater than 0.", nameof(lineNumber));

        if (string.IsNullOrWhiteSpace(accountCode))
            throw new ArgumentException("Account code must be provided.", nameof(accountCode));

        if (debitAmount < 0 || creditAmount < 0)
            throw new ArgumentException("Debit and credit amounts must be non-negative.");

        if (debitAmount == 0 && creditAmount == 0)
            throw new ArgumentException("Either debit or credit amount must be provided.");

        if (debitAmount > 0 && creditAmount > 0)
            throw new ArgumentException("Both debit and credit amounts cannot be specified simultaneously.");

        return new JournalEntry(
            Guid.NewGuid(),
            voucherId,
            lineNumber,
            accountCode,
            accountName,
            debitAmount,
            creditAmount,
            description);
    }

    /// <summary>
    /// Gets the amount (either debit or credit).
    /// </summary>
    public decimal Amount => DebitAmount > 0 ? DebitAmount : CreditAmount;

    /// <summary>
    /// Gets whether this is a debit entry.
    /// </summary>
    public bool IsDebit => DebitAmount > 0;

    /// <summary>
    /// Gets whether this is a credit entry.
    /// </summary>
    public bool IsCredit => CreditAmount > 0;
}
