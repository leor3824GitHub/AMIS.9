using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Catalog.Domain.Events;
using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Domain;

/// <summary>
/// Journal Entry Voucher (JEV) for COA-compliant accounting entries
/// Auto-generated for inventory transactions per COA Circular 2022-002
/// </summary>
public class JournalEntryVoucher : AuditableEntity, IAggregateRoot
{
    public string JEVNumber { get; private set; } = default!;
    public DateTime TransactionDate { get; private set; }
    public string Description { get; private set; } = default!;
    public Guid? SourceTransactionId { get; private set; }
    public string? SourceDocumentType { get; private set; } // RSMI, ICS, PAR, PO, etc.
    public string? SourceDocumentNumber { get; private set; }
    public decimal TotalDebit { get; private set; }
    public decimal TotalCredit { get; private; }
    public bool IsPosted { get; private set; }
    public DateTime? PostedDate { get; private set; }
    public Guid? PostedBy { get; private set; }
    public string? Notes { get; private set; }

    public virtual ICollection<JournalEntryLine> Lines { get; private set; } = new List<JournalEntryLine>();

    private JournalEntryVoucher() { }

    private JournalEntryVoucher(
        Guid id,
        string jevNumber,
        DateTime transactionDate,
        string description,
        Guid? sourceTransactionId,
        string? sourceDocumentType,
        string? sourceDocumentNumber,
        string? notes)
    {
        Id = id;
        JEVNumber = jevNumber;
        TransactionDate = transactionDate;
        Description = description;
        SourceTransactionId = sourceTransactionId;
        SourceDocumentType = sourceDocumentType;
        SourceDocumentNumber = sourceDocumentNumber;
        Notes = notes;
        IsPosted = false;
        TotalDebit = 0;
        TotalCredit = 0;

        QueueDomainEvent(new JournalEntryVoucherCreated { JournalEntryVoucher = this });
    }

    public static JournalEntryVoucher Create(
        string jevNumber,
        DateTime transactionDate,
        string description,
        Guid? sourceTransactionId = null,
        string? sourceDocumentType = null,
        string? sourceDocumentNumber = null,
        string? notes = null)
    {
        if (string.IsNullOrWhiteSpace(jevNumber))
            throw new ArgumentException("JEV number is required.", nameof(jevNumber));
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description is required.", nameof(description));

        return new JournalEntryVoucher(
            Guid.NewGuid(),
            jevNumber,
            transactionDate,
            description,
            sourceTransactionId,
            sourceDocumentType,
            sourceDocumentNumber,
            notes);
    }

    public void AddLine(string accountCode, string accountTitle, decimal debitAmount, decimal creditAmount, string? particulars = null)
    {
        if (IsPosted)
            throw new InvalidOperationException("Cannot modify a posted JEV.");

        if (debitAmount < 0 || creditAmount < 0)
            throw new ArgumentException("Debit and credit amounts cannot be negative.");

        if (debitAmount > 0 && creditAmount > 0)
            throw new ArgumentException("A line can be either debit or credit, not both.");

        if (debitAmount == 0 && creditAmount == 0)
            throw new ArgumentException("Either debit or credit amount must be greater than zero.");

        var line = new JournalEntryLine
        {
            Id = Guid.NewGuid(),
            JournalEntryVoucherId = this.Id,
            AccountCode = accountCode,
            AccountTitle = accountTitle,
            DebitAmount = debitAmount,
            CreditAmount = creditAmount,
            Particulars = particulars
        };

        Lines.Add(line);
        RecalculateTotals();

        QueueDomainEvent(new JournalEntryVoucherUpdated { JournalEntryVoucher = this });
    }

    public void Post(Guid postedBy)
    {
        if (IsPosted)
            throw new InvalidOperationException("JEV is already posted.");

        if (!IsBalanced)
            throw new InvalidOperationException("JEV must be balanced (Total Debit = Total Credit) before posting.");

        if (Lines.Count == 0)
            throw new InvalidOperationException("JEV must have at least one line before posting.");

        IsPosted = true;
        PostedDate = DateTime.UtcNow;
        PostedBy = postedBy;

        QueueDomainEvent(new JournalEntryVoucherPosted { JournalEntryVoucher = this });
    }

    public bool IsBalanced => Math.Abs(TotalDebit - TotalCredit) < 0.01m;

    private void RecalculateTotals()
    {
        TotalDebit = Lines.Sum(l => l.DebitAmount);
        TotalCredit = Lines.Sum(l => l.CreditAmount);
    }
}

/// <summary>
/// Journal Entry Line - individual debit/credit entries
/// </summary>
public class JournalEntryLine : BaseEntity
{
    public Guid JournalEntryVoucherId { get; set; }
    public string AccountCode { get; set; } = default!; // RCA 2019 account code
    public string AccountTitle { get; set; } = default!;
    public decimal DebitAmount { get; set; }
    public decimal CreditAmount { get; set; }
    public string? Particulars { get; set; }

    public virtual JournalEntryVoucher JournalEntryVoucher { get; set; } = default!;
}
