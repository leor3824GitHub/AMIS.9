//csharp api/modules/Catalog/Catalog.Domain/Events/InspectionApproved.cs
using System;
using AMIS.Framework.Core.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain.Events;

public sealed record InspectionApproved : DomainEvent
{
    public Guid InspectionId { get; init; }
    public Guid? InspectionRequestId { get; init; }
    public Guid EmployeeId { get; init; }
    public DateTime ApprovedOn { get; init; } = DateTime.UtcNow;
    public Guid PurchaseId { get; init; }
}
