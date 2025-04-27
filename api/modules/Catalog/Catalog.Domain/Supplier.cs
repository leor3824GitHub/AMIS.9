using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Catalog.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain;
public class Supplier : AuditableEntity, IAggregateRoot
{
    public string Name { get; private set; } = string.Empty;
    public string? Address { get; private set; }
    public string? Tin { get; private set; }
    public string TaxClassification { get; private set; } = string.Empty;
    public string? ContactNo { get; private set; }
    public string? Emailadd { get; private set; }

    private Supplier() { }

    private Supplier(Guid id, string name, string? address, string? tin, string taxClassification, string? contactNo, string? emailadd)
    {
        Id = id;
        Name = name;
        Address = address;
        Tin = tin;
        TaxClassification = taxClassification;
        ContactNo = contactNo;
        Emailadd = emailadd;
        QueueDomainEvent(new SupplierCreated { Supplier = this });
    }

    public static Supplier Create(string name, string? address, string? tin, string taxClassification, string? contactNo, string? emailadd)
    {
        return new Supplier(Guid.NewGuid(), name, address, tin, taxClassification, contactNo, emailadd);
    }

    public Supplier Update(string? name, string? address, string? tin, string taxClassification, string? contactNo, string? emailadd)
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

        if (!string.Equals(Tin, tin, StringComparison.OrdinalIgnoreCase))
        {
            Tin = tin;
            isUpdated = true;
        }

        if (!string.Equals(TaxClassification, taxClassification, StringComparison.OrdinalIgnoreCase))
        {
            TaxClassification = taxClassification;
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


