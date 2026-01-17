using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Inventories.Domain.Events;
public sealed record InspectionCreated : DomainEvent
{
    public Guid InspectionId { get; init; }
    public Guid? PurchaseId { get; init; }
    public Guid EmployeeId { get; init; }
}

