using System.Collections.Generic;
using System.Linq;
using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Catalog.Domain.Events;
using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Domain;

public enum IssuanceType
{
    PAR = 0,   // Property Acknowledgment Receipt (for PPE)
    ICS = 1    // Internal Control Slip (for Semi-Expendable)
}

public enum IssuanceStatus
{
    Pending = 0,     // Created, waiting for custodian acceptance
    Accepted = 1,    // Custodian accepted the asset
    Rejected = 2,    // Custodian rejected the issuance
    Returned = 3,    // Asset returned by custodian
    Cancelled = 4    // Issuance cancelled
}

public class Issuance : AuditableEntity, IAggregateRoot
{
    public Guid EmployeeId { get; private set; }
    public DateTime IssuanceDate { get; private set; }
    public decimal TotalAmount { get; private set; }
    public bool IsClosed { get; private set; }
    
    // New acceptance workflow properties
    public IssuanceType Type { get; private set; } = IssuanceType.PAR;
    public Guid? CustodianId { get; private set; }
    public IssuanceStatus Status { get; private set; } = IssuanceStatus.Pending;
    public DateTime? AcceptedOn { get; private set; }
    public string? RejectionReason { get; private set; }
    public DigitalSignature? AcceptanceSignature { get; private set; }

    public virtual Employee Employee { get; private set; } = default!;
    public virtual ICollection<IssuanceItem> Items { get; private set; } = [];

    private Issuance() { }

    private Issuance(Guid id, Guid employeeId, DateTime issuanceDate, decimal totalAmount, IssuanceType type = IssuanceType.PAR)
    {
        Id = id;
        EmployeeId = employeeId;
        IssuanceDate = issuanceDate;
        TotalAmount = totalAmount;
        Type = type;
        IsClosed = false;

        QueueDomainEvent(new IssuanceCreated { Issuance = this });
    }

    public static Issuance Create(Guid employeeId, DateTime issuanceDate, decimal totalAmount, IssuanceType type = IssuanceType.PAR)
    {
        EnsureNonNegative(totalAmount);
        return new Issuance(Guid.NewGuid(), employeeId, issuanceDate, totalAmount, type);
    }

    public Issuance Update(Guid employeeId, DateTime issuanceDate, decimal totalAmount, bool isClosed)
    {
        bool hasChanges = false;

        if (IsClosed != isClosed)
        {
            if (isClosed)
            {
                EnsureCanClose();
            }

            IsClosed = isClosed;
            hasChanges = true;
        }

        if (IsClosed)
        {
            if (EmployeeId != employeeId || IssuanceDate != issuanceDate)
            {
                throw new InvalidOperationException("Cannot modify a closed issuance. Reopen before making changes.");
            }

            if (TotalAmount != totalAmount)
            {
                throw new InvalidOperationException("Cannot change total amount on a closed issuance.");
            }

            if (hasChanges)
            {
                QueueDomainEvent(new IssuanceUpdated { Issuance = this });
            }

            return this;
        }

        if (EmployeeId != employeeId)
        {
            EmployeeId = employeeId;
            hasChanges = true;
        }

        if (IssuanceDate != issuanceDate)
        {
            IssuanceDate = issuanceDate;
            hasChanges = true;
        }

        if (TotalAmount != totalAmount)
        {
            EnsureNonNegative(totalAmount);
            TotalAmount = totalAmount;
            hasChanges = true;
        }

        if (hasChanges)
        {
            QueueDomainEvent(new IssuanceUpdated { Issuance = this });
        }

        return this;
    }

    public void RegisterItemAdded(int qty, decimal unitPrice)
    {
        EnsureMutable();
        ValidateLine(qty, unitPrice);

        var increment = CalculateLineAmount(qty, unitPrice);
        if (increment <= 0)
        {
            return;
        }

        TotalAmount += increment;
        QueueDomainEvent(new IssuanceUpdated { Issuance = this });
    }

    public void RegisterItemUpdated(int originalQty, decimal originalUnitPrice, int newQty, decimal newUnitPrice)
    {
        EnsureMutable();
        ValidateLine(newQty, newUnitPrice);
        if (originalQty <= 0)
        {
            throw new ArgumentException("Original quantity must be greater than zero.", nameof(originalQty));
        }

        if (originalUnitPrice < 0)
        {
            throw new ArgumentException("Original unit price must not be negative.", nameof(originalUnitPrice));
        }

        var delta = CalculateLineAmount(newQty, newUnitPrice) - CalculateLineAmount(originalQty, originalUnitPrice);
        if (delta == 0)
        {
            return;
        }

        var candidate = TotalAmount + delta;
        EnsureNonNegative(candidate);
        TotalAmount = candidate;
        QueueDomainEvent(new IssuanceUpdated { Issuance = this });
    }

    public void RegisterItemRemoved(int qty, decimal unitPrice)
    {
        EnsureMutable();
        if (qty <= 0)
        {
            throw new ArgumentException("Quantity must be greater than zero.", nameof(qty));
        }

        if (unitPrice < 0)
        {
            throw new ArgumentException("Unit price must not be negative.", nameof(unitPrice));
        }

        var deduction = CalculateLineAmount(qty, unitPrice);
        var candidate = TotalAmount - deduction;
        TotalAmount = candidate < 0 ? 0 : candidate;
        QueueDomainEvent(new IssuanceUpdated { Issuance = this });
    }

    public void RecalculateTotals(IEnumerable<IssuanceItem> items)
    {
        var recomputed = items?.Sum(i => CalculateLineAmount(i.Qty, i.UnitPrice)) ?? 0m;
        EnsureNonNegative(recomputed);

        if (TotalAmount != recomputed)
        {
            TotalAmount = recomputed;
            QueueDomainEvent(new IssuanceUpdated { Issuance = this });
        }
    }

    public void Close()
    {
        if (IsClosed)
        {
            return;
        }

        EnsureCanClose();
        IsClosed = true;
        QueueDomainEvent(new IssuanceUpdated { Issuance = this });
    }

    public void Reopen()
    {
        if (!IsClosed)
        {
            return;
        }

        IsClosed = false;
        QueueDomainEvent(new IssuanceUpdated { Issuance = this });
    }

    /// <summary>
    /// Marks the issuance as accepted by the custodian with digital signature.
    /// </summary>
    public void Accept(DigitalSignature signature)
    {
        if (Status != IssuanceStatus.Pending)
            throw new InvalidOperationException($"Cannot accept an issuance with status {Status}.");

        if (signature == null)
            throw new ArgumentNullException(nameof(signature));

        Status = IssuanceStatus.Accepted;
        AcceptedOn = DateTime.UtcNow;
        AcceptanceSignature = signature;
        CustodianId = signature.SignedByEmployeeId;

        QueueDomainEvent(new IssuanceAccepted { Issuance = this });
    }

    /// <summary>
    /// Marks the issuance as rejected by the custodian with a reason.
    /// </summary>
    public void Reject(string reason)
    {
        if (Status != IssuanceStatus.Pending)
            throw new InvalidOperationException($"Cannot reject an issuance with status {Status}.");

        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Rejection reason must be provided.", nameof(reason));

        Status = IssuanceStatus.Rejected;
        RejectionReason = reason;

        QueueDomainEvent(new IssuanceRejected { Issuance = this });
    }

    /// <summary>
    /// Marks the issuance as cancelled.
    /// </summary>
    public void Cancel()
    {
        if (Status == IssuanceStatus.Returned || Status == IssuanceStatus.Cancelled)
            throw new InvalidOperationException($"Cannot cancel an issuance with status {Status}.");

        Status = IssuanceStatus.Cancelled;

        QueueDomainEvent(new IssuanceCancelled { Issuance = this });
    }

    /// <summary>
    /// Marks the asset as returned.
    /// </summary>
    public void MarkAsReturned()
    {
        if (Status != IssuanceStatus.Accepted)
            throw new InvalidOperationException("Only accepted issuances can be marked as returned.");

        Status = IssuanceStatus.Returned;

        QueueDomainEvent(new IssuanceReturned { Issuance = this });
    }

    /// <summary>
    /// Sets the issuance type (PAR or ICS).
    /// </summary>
    public void SetType(IssuanceType type)
    {
        if (IsClosed)
            throw new InvalidOperationException("Cannot modify a closed issuance.");

        Type = type;
    }

    private void EnsureMutable()
    {
        if (IsClosed)
        {
            throw new InvalidOperationException("Issuance is closed and cannot be modified.");
        }
    }

    private void EnsureCanClose()
    {
        if (TotalAmount <= 0)
        {
            throw new InvalidOperationException("Cannot close an issuance without any issued items.");
        }
    }

    private static void ValidateLine(int qty, decimal unitPrice)
    {
        if (qty <= 0)
        {
            throw new ArgumentException("Quantity must be greater than zero.", nameof(qty));
        }

        if (unitPrice < 0)
        {
            throw new ArgumentException("Unit price must not be negative.", nameof(unitPrice));
        }
    }

    private static decimal CalculateLineAmount(int qty, decimal unitPrice) => qty * unitPrice;

    private static void EnsureNonNegative(decimal value)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "Amount must not be negative.");
        }
    }
}

