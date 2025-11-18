using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Catalog.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain;
public class Employee : AuditableEntity, IAggregateRoot
{
    public string Name { get; private set; } = string.Empty;
    public string Designation { get; private set; } = string.Empty;
    public string ResponsibilityCode { get; private set; } = string.Empty;
    public Guid? UserId { get; private set; }

    private Employee() { }

    private Employee(Guid id, string name, string designation, string responsibilityCode, Guid? userId)
    {
        Id = id;
        Name = name;
        Designation = designation;
        ResponsibilityCode = responsibilityCode;
        UserId = userId;
        QueueDomainEvent(new EmployeeCreated { Employee = this });
    }

    public static Employee Create(string name, string designation, string responsibilityCode, Guid? userId)
    {
        return new Employee(Guid.NewGuid(), name, designation, responsibilityCode, userId);
    }

    public Employee Update(string? name, string? designation, string? responsibilityCode, Guid? userId)
    {
        bool isUpdated = false;

        if (!string.IsNullOrWhiteSpace(name) && !string.Equals(Name, name, StringComparison.OrdinalIgnoreCase))
        {
            Name = name;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(designation) && !string.Equals(Designation, designation, StringComparison.OrdinalIgnoreCase))
        {
            Designation = designation;
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(responsibilityCode) && !string.Equals(ResponsibilityCode, responsibilityCode, StringComparison.OrdinalIgnoreCase))
        {
            ResponsibilityCode = responsibilityCode;
            isUpdated = true;
        }

        if (UserId != userId)
        {
            UserId = userId;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new EmployeeUpdated { Employee = this });
        }

        return this;
    }
}


