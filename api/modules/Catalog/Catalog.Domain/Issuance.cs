using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Catalog.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain;
public class Issuance : AuditableEntity, IAggregateRoot
{
    public Guid ProductId { get; private set; }
    public Guid EmployeeId { get; private set; }
    public decimal Qty { get; private set; }
    public string Unit { get; private set; } = string.Empty; // Initialize with a default value
    public decimal UnitPrice { get; private set; }
    public virtual Product Product { get; private set; } = default!;
    public virtual Employee Employee { get; private set; } = default!;

    private Issuance() { }

    private Issuance(Guid id, Guid productId, Guid employeeId, decimal qty, string unit, decimal unitPrice)
    {
        Id = id;
        ProductId = productId;
        EmployeeId = employeeId;
        Qty = qty;
        Unit = unit;
        UnitPrice = unitPrice;

        QueueDomainEvent(new IssuanceUpdated { Issuance = this });
    }

    public static Issuance Create(Guid productId, Guid employeeId, decimal qty, string unit, decimal unitPrice)
    {
        return new Issuance(Guid.NewGuid(), productId, employeeId, qty, unit, unitPrice);
    }

    public Issuance Update(Guid productId, Guid employeeId, decimal qty, string unit, decimal unitPrice)
    {
        bool isUpdated = false;

        if (ProductId != productId)
        {
            ProductId = productId;
            isUpdated = true;
        }

        if (EmployeeId != employeeId)
        {
            EmployeeId = employeeId;
            isUpdated = true;
        }

        if (Qty != qty)
        {
            Qty = qty;
            isUpdated = true;
        }

        if (!string.Equals(Unit, unit, StringComparison.OrdinalIgnoreCase))
        {
            Unit = unit;
            isUpdated = true;
        }

        if (UnitPrice != unitPrice)
        {
            UnitPrice = unitPrice;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new IssuanceUpdated { Issuance = this });
        }

        return this;
    }
}

