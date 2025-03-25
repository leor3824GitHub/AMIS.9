using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain.Events;
public sealed record EmployeeCreated : DomainEvent
{
    public Employee? Employee { get; set; }
}
