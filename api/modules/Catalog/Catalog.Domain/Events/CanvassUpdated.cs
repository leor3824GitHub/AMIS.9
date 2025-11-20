using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain.Events;

public sealed record CanvassUpdated : DomainEvent
{
    public Canvass? Canvass { get; set; }
}
