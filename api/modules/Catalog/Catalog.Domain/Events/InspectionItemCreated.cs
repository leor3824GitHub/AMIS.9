using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain.Events;
public sealed record InspectionItemCreated : DomainEvent
{
    public InspectionItem? InspectionItem { get; set; }
}
