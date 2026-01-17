using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Inventories.Domain.Events;
public sealed record EmployeeCreated : DomainEvent
{
    public Employee? Employee { get; set; }
}

