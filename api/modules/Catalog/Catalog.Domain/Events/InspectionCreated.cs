using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain.Events;
public sealed record InspectionCreated : DomainEvent
{
    public Inspection? Inspection { get; set; }
}
