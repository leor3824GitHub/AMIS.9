using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Inventories.Domain.Events;
public sealed record InspectionUpdated : DomainEvent
{
    public Inspection? Inspection { get; set; }
}

