using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain.Events;
public sealed record InspectionCreated : DomainEvent
{
    public Guid InspectionId { get; init; }
    public Guid InspectionRequestId { get; init; }
    public Guid EmployeeId { get; init; }
}
