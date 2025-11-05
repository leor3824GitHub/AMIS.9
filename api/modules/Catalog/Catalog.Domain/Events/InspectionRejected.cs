//csharp api/modules/Catalog/Catalog.Domain/Events/InspectionRejected.cs
using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain.Events;

public sealed record InspectionRejected : DomainEvent
{
    public Guid InspectionId { get; init; }
    public Guid InspectionRequestId { get; init; }
    public Guid EmployeeId { get; init; }
    public DateTime RejectedOn { get; init; } = DateTime.UtcNow;
    public string? Reason { get; init; }
}
