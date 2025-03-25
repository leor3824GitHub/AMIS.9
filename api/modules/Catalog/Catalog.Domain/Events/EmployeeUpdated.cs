using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain.Events;
public sealed record EmployeeUpdated : DomainEvent
{
    public Employee? Employee { get; set; }
}
