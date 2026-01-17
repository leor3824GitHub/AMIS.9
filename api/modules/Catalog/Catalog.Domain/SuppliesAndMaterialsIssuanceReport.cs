using AMIS.Framework.Core.Domain;

namespace AMIS.WebApi.Catalog.Domain;

/// <summary>
/// Represents a Supplies and Materials Issuance Report (SMIR) from the National Food Authority (NFA)
/// This is an official form used to track the movement and accountability of inventory
/// when supplies or materials are moved from a warehouse or office to another party.
/// </summary>
public class SuppliesAndMaterialsIssuanceReport : AuditableEntity
{
    // Transaction Header
    public string SmirNumber { get; private set; } // SMIR Number for tracking
    public DateTime TransactionDate { get; private set; }
    public RecipientInfo Recipient { get; private set; }
    public IssuanceReason IssuanceReason { get; private set; }

    // Line Items
    private readonly List<IssuanceLineItem> _lineItems = [];
    public IReadOnlyCollection<IssuanceLineItem> LineItems => _lineItems.AsReadOnly();

    // Authorization & Receipt
    public IssuanceAuthorization Authorization { get; private set; }

    // Distribution Tracking
    public bool DistributedToRecipient { get; private set; }
    public bool DistributedToPMSDS { get; private set; } // Property Management and Sales Development Section
    public bool DistributedToAccounting { get; private set; }
    public bool DistributedToAccountingAdvice { get; private set; } // For internal financial alerts/budgeting
    public bool DistributedToFile { get; private set; } // Originating office copy

    // Additional metadata
    public string? Notes { get; private set; }

    private SuppliesAndMaterialsIssuanceReport() { }

    /// <summary>
    /// Creates a new Supplies and Materials Issuance Report
    /// </summary>
    public SuppliesAndMaterialsIssuanceReport(
        string smirNumber,
        DateTime transactionDate,
        RecipientInfo recipient,
        IssuanceReason issuanceReason,
        IssuanceAuthorization authorization,
        string? notes = null)
    {
        if (string.IsNullOrWhiteSpace(smirNumber))
            throw new ArgumentException("SMIR number cannot be empty.", nameof(smirNumber));

        if (transactionDate > DateTime.UtcNow)
            throw new ArgumentException("Transaction date cannot be in the future.", nameof(transactionDate));

        Id = Guid.NewGuid();
        SmirNumber = smirNumber;
        TransactionDate = transactionDate;
        Recipient = recipient ?? throw new ArgumentNullException(nameof(recipient));
        IssuanceReason = issuanceReason ?? throw new ArgumentNullException(nameof(issuanceReason));
        Authorization = authorization ?? throw new ArgumentNullException(nameof(authorization));
        Notes = notes;
    }

    /// <summary>
    /// Adds a line item to the issuance report
    /// </summary>
    public void AddLineItem(IssuanceLineItem lineItem)
    {
        if (lineItem == null)
            throw new ArgumentNullException(nameof(lineItem));

        _lineItems.Add(lineItem);
    }

    /// <summary>
    /// Adds multiple line items to the issuance report
    /// </summary>
    public void AddLineItems(IEnumerable<IssuanceLineItem> lineItems)
    {
        if (lineItems == null)
            throw new ArgumentNullException(nameof(lineItems));

        foreach (var item in lineItems)
        {
            AddLineItem(item);
        }
    }

    /// <summary>
    /// Removes a line item from the issuance report
    /// </summary>
    public void RemoveLineItem(IssuanceLineItem lineItem)
    {
        _lineItems.Remove(lineItem);
    }

    /// <summary>
    /// Gets the total amount of all line items
    /// </summary>
    public decimal GetTotalAmount() => _lineItems.Sum(x => x.Amount);

    /// <summary>
    /// Marks the report as distributed to the recipient
    /// </summary>
    public void MarkDistributedToRecipient()
    {
        DistributedToRecipient = true;
    }

    /// <summary>
    /// Marks the report as distributed to PMSDS
    /// </summary>
    public void MarkDistributedToPMSDS()
    {
        DistributedToPMSDS = true;
    }

    /// <summary>
    /// Marks the report as distributed to Accounting
    /// </summary>
    public void MarkDistributedToAccounting()
    {
        DistributedToAccounting = true;
    }

    /// <summary>
    /// Marks the report as distributed to Accounting (Advice)
    /// </summary>
    public void MarkDistributedToAccountingAdvice()
    {
        DistributedToAccountingAdvice = true;
    }

    /// <summary>
    /// Marks the report as filed
    /// </summary>
    public void MarkDistributedToFile()
    {
        DistributedToFile = true;
    }

    /// <summary>
    /// Marks all distribution channels as completed
    /// </summary>
    public void MarkAllDistributionComplete()
    {
        DistributedToRecipient = true;
        DistributedToPMSDS = true;
        DistributedToAccounting = true;
        DistributedToAccountingAdvice = true;
        DistributedToFile = true;
    }

    /// <summary>
    /// Gets distribution status summary
    /// </summary>
    public IReadOnlyDictionary<string, bool> GetDistributionStatus() => new Dictionary<string, bool>
    {
        { "Recipient", DistributedToRecipient },
        { "PMSDS", DistributedToPMSDS },
        { "Accounting", DistributedToAccounting },
        { "AccountingAdvice", DistributedToAccountingAdvice },
        { "File", DistributedToFile }
    };

    /// <summary>
    /// Updates the notes field
    /// </summary>
    public void UpdateNotes(string? notes)
    {
        Notes = notes;
    }
}
