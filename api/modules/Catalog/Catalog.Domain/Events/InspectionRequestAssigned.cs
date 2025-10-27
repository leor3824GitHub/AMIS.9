using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain.Events
{
    public sealed record InspectionRequestAssigned : DomainEvent
    {
        public Guid RequestId { get; init; }
        public Guid InspectorId { get; init; }
    }
}

