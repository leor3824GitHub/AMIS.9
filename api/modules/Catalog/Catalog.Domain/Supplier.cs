using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Catalog.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain;
public class Supplier : AuditableEntity, IAggregateRoot
{
    public string Name { get; private set; } = string.Empty;
    public string? Address { get; private set; }
    public string? TIN { get; private set; }
    public bool IsVAT { get; private set; } = true;
    public string? ContactNo { get; private set; }
    public string? Emailadd { get; private set; }

    private Supplier() { }

    private Supplier(Guid id, string name, string? address, string? tin, bool isVAT, string? contactNo, string? emailadd)
    {
        Id = id;
        Name = name;
        Address = address;
        TIN = tin;
        IsVAT = isVAT;
        ContactNo = contactNo;
        Emailadd = emailadd;
        QueueDomainEvent(new SupplierCreated { Supplier = this });
    }

    public static Supplier Create(string name, string? address, string? tin, bool isVAT, string? contactNo, string? emailadd)
    {
        return new Supplier(Guid.NewGuid(), name, address, tin, isVAT, contactNo, emailadd);
    }

    public Supplier Update(string? name, string? address, string? tin, bool isVAT, string? contactNo, string? emailadd)
    {
        bool isUpdated = false;

        if (!string.IsNullOrWhiteSpace(name) && !string.Equals(Name, name, StringComparison.OrdinalIgnoreCase))
        {
            Name = name;
            isUpdated = true;
        }

        if (!string.Equals(Address, address, StringComparison.OrdinalIgnoreCase))
        {
            Address = address;
            isUpdated = true;
        }

        if (!string.Equals(TIN, tin, StringComparison.OrdinalIgnoreCase))
        {
            TIN = tin;
            isUpdated = true;
        }

        if (IsVAT != isVAT)
        {
            IsVAT = isVAT;
            isUpdated = true;
        }

        if (!string.Equals(ContactNo, contactNo, StringComparison.OrdinalIgnoreCase))
        {
            ContactNo = contactNo;
            isUpdated = true;
        }

        if (!string.Equals(Emailadd, emailadd, StringComparison.OrdinalIgnoreCase))
        {
            Emailadd = emailadd;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new SupplierUpdated { Supplier = this });
        }

        return this;
    }
}


