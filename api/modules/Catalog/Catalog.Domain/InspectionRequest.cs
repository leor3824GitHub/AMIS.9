using AMIS.Framework.Core.Domain.Contracts;
using AMIS.Framework.Core.Domain;
using AMIS.WebApi.Catalog.Domain.ValueObjects;
using AMIS.WebApi.Catalog.Domain.Events;

namespace AMIS.WebApi.Catalog.Domain;

public class InspectionRequest : AuditableEntity, IAggregateRoot
{
    public Guid PurchaseId { get; private set; }
    public Guid? RequestedById { get; private set; } // Warehouse user
    public Guid? AssignedInspectorId { get; private set; }
    public InspectionRequestStatus Status { get; private set; }

    public InspectionRequest(Guid purchaseId, Guid? requestedById)
    {
        Id = Guid.NewGuid();
        PurchaseId = purchaseId;
        RequestedById = requestedById;
        Status = InspectionRequestStatus.Pending;
    }

    public void AssignInspector(Guid inspectorId)
    {
        AssignedInspectorId = inspectorId;
        Status = InspectionRequestStatus.Assigned;
        QueueDomainEvent(new InspectionRequestAssigned { RequestId = Id, InspectorId = inspectorId });
    }

    public void MarkCompleted()
    {
        Status = InspectionRequestStatus.Completed;
    }
}
