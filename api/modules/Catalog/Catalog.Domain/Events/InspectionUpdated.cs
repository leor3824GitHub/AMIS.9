using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain.Events;
public sealed record InspectionUpdated : DomainEvent
{
    public Inspection? Inspection { get; set; }
}
