using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain.Events;
public sealed record BrandUpdated : DomainEvent
{
    public Brand? Brand { get; set; }
}
