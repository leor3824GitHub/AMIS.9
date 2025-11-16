using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.Framework.Core.Exceptions;
using AMIS.WebApi.Catalog.Domain.Events;
using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Domain;
public class Employee : AuditableEntity, IAggregateRoot
{
    public string Name { get; private set; } = string.Empty;
    public string Designation { get; private set; } = string.Empty;
    public ResponsibilityCode ResponsibilityCode { get; private set; } = default!;
    public string Department { get; private set; } = string.Empty;
    public ContactInformation ContactInfo { get; private set; } = default!;
    public EmploymentStatus Status { get; private set; } = EmploymentStatus.Active;
    public DateTime? HireDate { get; private set; }
    public DateTime? TerminationDate { get; private set; }
    public Guid? SupervisorId { get; private set; }
    public Guid? UserId { get; private set; }

    private Employee() { }

    private Employee(
        Guid id,
        string name,
        string designation,
        ResponsibilityCode responsibilityCode,
        string department,
        ContactInformation contactInfo,
        EmploymentStatus status,
        DateTime? hireDate,
        Guid? supervisorId,
        Guid? userId)
    {
        Id = id;
        Name = name;
        Designation = designation;
        ResponsibilityCode = responsibilityCode;
        Department = department;
        ContactInfo = contactInfo;
        Status = status;
        HireDate = hireDate;
        SupervisorId = supervisorId;
        UserId = userId;
        QueueDomainEvent(new EmployeeCreated { Employee = this });
    }

    public static Employee Create(
        string name,
        string designation,
        string responsibilityCode,
        string department,
        string email,
        string phoneNumber,
        DateTime? hireDate = null,
        Guid? supervisorId = null,
        Guid? userId = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new FshException("Employee name cannot be empty");

        if (string.IsNullOrWhiteSpace(designation))
            throw new FshException("Employee designation cannot be empty");

        if (string.IsNullOrWhiteSpace(department))
            throw new FshException("Employee department cannot be empty");

        var respCode = ResponsibilityCode.Create(responsibilityCode);
        var contactInfo = ContactInformation.Create(email, phoneNumber);

        return new Employee(
            Guid.NewGuid(),
            name.Trim(),
            designation.Trim(),
            respCode,
            department.Trim(),
            contactInfo,
            EmploymentStatus.Active,
            hireDate ?? DateTime.UtcNow,
            supervisorId,
            userId);
    }

    public Employee Update(
        string? name,
        string? designation,
        string? responsibilityCode,
        string? department,
        string? email,
        string? phoneNumber,
        DateTime? hireDate,
        Guid? supervisorId,
        Guid? userId)
    {
        bool isUpdated = false;

        if (!string.IsNullOrWhiteSpace(name) && !string.Equals(Name, name, StringComparison.OrdinalIgnoreCase))
        {
            Name = name.Trim();
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(designation) && !string.Equals(Designation, designation, StringComparison.OrdinalIgnoreCase))
        {
            Designation = designation.Trim();
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(responsibilityCode))
        {
            var newCode = ResponsibilityCode.Create(responsibilityCode);
            string currentCode = ResponsibilityCode;
            string proposedCode = newCode;
            if (!string.Equals(currentCode, proposedCode, StringComparison.OrdinalIgnoreCase))
            {
                ResponsibilityCode = newCode;
                isUpdated = true;
            }
        }

        if (!string.IsNullOrWhiteSpace(department) && !string.Equals(Department, department, StringComparison.OrdinalIgnoreCase))
        {
            Department = department.Trim();
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(email) && !string.Equals(ContactInfo.Email, email.Trim(), StringComparison.OrdinalIgnoreCase))
        {
            ContactInfo = ContactInfo.UpdateEmail(email);
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(phoneNumber) && ContactInfo.PhoneNumber != phoneNumber.Trim())
        {
            ContactInfo = ContactInfo.UpdatePhoneNumber(phoneNumber);
            isUpdated = true;
        }

        if (hireDate.HasValue && HireDate != hireDate)
        {
            HireDate = hireDate;
            isUpdated = true;
        }

        if (SupervisorId != supervisorId)
        {
            SupervisorId = supervisorId;
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

    public Employee Terminate(DateTime? terminationDate = null)
    {
        if (Status == EmploymentStatus.Terminated)
            throw new FshException("Employee is already terminated");

        Status = EmploymentStatus.Terminated;
        TerminationDate = terminationDate ?? DateTime.UtcNow;
        QueueDomainEvent(new EmployeeUpdated { Employee = this });
        return this;
    }

    public Employee Reactivate()
    {
        if (Status != EmploymentStatus.Terminated && Status != EmploymentStatus.Inactive)
            throw new FshException("Only terminated or inactive employees can be reactivated");

        Status = EmploymentStatus.Active;
        TerminationDate = null;
        QueueDomainEvent(new EmployeeUpdated { Employee = this });
        return this;
    }

    public Employee SetInactive()
    {
        if (Status == EmploymentStatus.Terminated)
            throw new FshException("Cannot set terminated employee to inactive");

        Status = EmploymentStatus.Inactive;
        QueueDomainEvent(new EmployeeUpdated { Employee = this });
        return this;
    }
}


