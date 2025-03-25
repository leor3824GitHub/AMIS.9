using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain.Events;
public sealed record SupplierUpdated : DomainEvent
{
    public Supplier? Supplier { get; set; }
}
