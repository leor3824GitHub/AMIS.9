using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Inventories.Domain.Events;
public sealed record InspectionItemUpdated : DomainEvent
{
    public InspectionItem? InspectionItem { get; set; }
}

