using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Inventories.Domain.Events;
public sealed record BrandCreated : DomainEvent
{
    public Brand? Brand { get; set; }
}

