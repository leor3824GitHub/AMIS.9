using System.Collections.Generic;
using System.Linq;
using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Catalog.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain;
public class Issuance : AuditableEntity, IAggregateRoot
{
    public Guid EmployeeId { get; private set; }
    public DateTime IssuanceDate { get; private set; }
    public decimal TotalAmount { get; private set; }
    public bool IsClosed { get; private set; }
    public virtual Employee Employee { get; private set; } = default!;
    public virtual ICollection<IssuanceItem> Items { get; private set; } = [];

    private Issuance() { }

    private Issuance(Guid id, Guid employeeId, DateTime issuanceDate, decimal totalAmount)
    {
        Id = id;
        EmployeeId = employeeId;
        IssuanceDate = issuanceDate;
        TotalAmount = totalAmount;
        IsClosed = false;

        QueueDomainEvent(new IssuanceCreated { Issuance = this });
    }

    public static Issuance Create(Guid employeeId, DateTime issuanceDate, decimal totalAmount)
    {
        EnsureNonNegative(totalAmount);
        return new Issuance(Guid.NewGuid(), employeeId, issuanceDate, totalAmount);
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

