using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain.Events;
public sealed record InspectionItemUpdated : DomainEvent
{
    public InspectionItem? InspectionItem { get; set; }
}
