using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Inventories.Domain.Events
{
    public sealed record InspectionRequestAssigned : DomainEvent
    {
        public Guid RequestId { get; init; }
        public Guid InspectorId { get; init; }
    }
}


