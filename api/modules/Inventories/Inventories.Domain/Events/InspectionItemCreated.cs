using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Inventories.Domain.Events;
public sealed record InspectionItemCreated : DomainEvent
{
    public InspectionItem? InspectionItem { get; set; }
}

