using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;

namespace AMIS.WebApi.Inventories.Domain;

/// <summary>
/// Represents a Supplies and Materials Receiving Report (SMRR) from the National Food Authority (NFA)
/// This is an official form used to document the arrival and intake of goods into the NFA's inventory.
/// It serves as the counterpart to the Issuance Report (SMIR).
/// </summary>
public class SuppliesAndMaterialsReceivingReport : AuditableEntity, IAggregateRoot
{
    // Header and Tracking
    public string SmrrNumber { get; private set; } // SMRR Number for tracking
    public string Location { get; private set; } // NFA location (e.g., Quezon City office)

    // Source Information
    public ReceivingSourceInfo Source { get; private set; }

    // Nature of Transaction
    public ReceivingTransactionType TransactionType { get; private set; }

    // Line Items
    private readonly List<ReceivingLineItem> _lineItems = [];
    public IReadOnlyCollection<ReceivingLineItem> LineItems => _lineItems.AsReadOnly();

    // Authentication
    public ReceivingAuthentication Authentication { get; private set; }

    // Distribution Tracking
    public bool DistributedToVoucher { get; private set; } // Copy 1: Voucher or Issuing Party
    public bool DistributedToPMSDS { get; private set; } // Copy 2: Property Management and Sales Development Section
    public bool DistributedToAccounting { get; private set; } // Copy 3: Accounting
    public bool DistributedToFile { get; private set; } // Copy 4: File (local record keeping)

    // Additional metadata
    public string? Notes { get; private set; }

    private SuppliesAndMaterialsReceivingReport()
    {
        SmrrNumber = string.Empty;
        Location = string.Empty;
        Source = null!;
        TransactionType = null!;
        Authentication = null!;
    }

    /// <summary>
    /// Creates a new Supplies and Materials Receiving Report
    /// </summary>
    public SuppliesAndMaterialsReceivingReport(
        string smrrNumber,
        string location,
        ReceivingSourceInfo source,
        ReceivingTransactionType transactionType,
        ReceivingAuthentication authentication,
        string? notes = null)
    {
        if (string.IsNullOrWhiteSpace(smrrNumber))
            throw new ArgumentException("SMRR number cannot be empty.", nameof(smrrNumber));

        if (string.IsNullOrWhiteSpace(location))
            throw new ArgumentException("Location cannot be empty.", nameof(location));

        Id = Guid.NewGuid();
        SmrrNumber = smrrNumber;
        ArgumentNullException.ThrowIfNull(location);
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(transactionType);
        ArgumentNullException.ThrowIfNull(authentication);
        
        SmrrNumber = smrrNumber;
        Location = location;
        Source = source;
        TransactionType = transactionType;
        Authentication = authentication;
        Notes = notes;
    }

    /// <summary>
    /// Adds a line item to the receiving report
    /// </summary>
    public void AddLineItem(ReceivingLineItem lineItem)
    {
        ArgumentNullException.ThrowIfNull(lineItem);
        _lineItems.Add(lineItem);
    }

    /// <summary>
    /// Adds multiple line items to the receiving report
    /// </summary>
    public void AddLineItems(IEnumerable<ReceivingLineItem> lineItems)
    {
        ArgumentNullException.ThrowIfNull(lineItems);

        foreach (var item in lineItems)
        {
            AddLineItem(item);
        }
    }

    /// <summary>
    /// Removes a line item from the receiving report
    /// </summary>
    public void RemoveLineItem(ReceivingLineItem lineItem)
    {
        _lineItems.Remove(lineItem);
    }

    /// <summary>
    /// Gets the total amount of all line items
    /// </summary>
    public decimal GetTotalAmount() => _lineItems.Sum(x => x.Amount);

    /// <summary>
    /// Marks the report as distributed to the voucher/issuing party
    /// </summary>
    public void MarkDistributedToVoucher()
    {
        DistributedToVoucher = true;
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
    /// Marks the report as filed locally
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
        DistributedToVoucher = true;
        DistributedToPMSDS = true;
        DistributedToAccounting = true;
        DistributedToFile = true;
    }

    /// <summary>
    /// Gets distribution status summary
    /// </summary>
    public IReadOnlyDictionary<string, bool> GetDistributionStatus() => new Dictionary<string, bool>
    {
        { "Voucher", DistributedToVoucher },
        { "PMSDS", DistributedToPMSDS },
        { "Accounting", DistributedToAccounting },
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

