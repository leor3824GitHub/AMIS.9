using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Inventories.Domain.Events;
public sealed record InspectionRequestCreated : DomainEvent
{
    public Guid RequestId { get; set; }
}
public sealed record InspectionRequestUpdated : DomainEvent
{
    public Guid RequestId { get; set; }
}

public sealed record InspectionRequestCompleted : DomainEvent
{
    public Guid RequestId { get; set; }
}

