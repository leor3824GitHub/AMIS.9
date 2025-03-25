using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Catalog.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain;
public class Issuance : AuditableEntity, IAggregateRoot
{
    public Guid EmployeeId { get; private set; }
    public DateTime IssuanceDate { get; private set; }
    public decimal TotalAmount { get; private set; }
    public string? Status { get; private set; }
    public virtual Employee Employee { get; private set; } = default!;

    private Issuance() { }

    private Issuance(Guid id, Guid employeeId, decimal totalAmount, string? status)
    {
        Id = id;
        EmployeeId = employeeId;
        IssuanceDate = DateTime.UtcNow;
        TotalAmount = totalAmount;
        Status = status;

        QueueDomainEvent(new IssuanceUpdated { Issuance = this });
    }

    public static Issuance Create(Guid employeeId, decimal totalAmount, string? status)
    {
        return new Issuance(Guid.NewGuid(), employeeId, totalAmount, status);
    }

    public Issuance Update(Guid employeeId, DateTime issuanceDate, decimal totalAmount, string? status)
    {
        bool isUpdated = false;

        if (EmployeeId != employeeId)
        {
            EmployeeId = employeeId;
            isUpdated = true;
        }

        if (IssuanceDate != issuanceDate)
        {
            IssuanceDate = issuanceDate;
            isUpdated = true;
        }

        if (TotalAmount != totalAmount)
        {
            TotalAmount = totalAmount;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new IssuanceUpdated { Issuance = this });
        }

        return this;
    }
}

