using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain.Events;
public sealed record BrandCreated : DomainEvent
{
    public Brand? Brand { get; set; }
}
