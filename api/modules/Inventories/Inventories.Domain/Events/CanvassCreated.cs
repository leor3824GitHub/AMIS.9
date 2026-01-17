using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Inventories.Domain.Events;

public sealed record CanvassCreated : DomainEvent
{
    public Canvass? Canvass { get; set; }
}

