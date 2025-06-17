using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Catalog.Domain.Events;
using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Domain;

public class InspectionRequest : AuditableEntity, IAggregateRoot
{
    public Guid? PurchaseId { get; private set; }
    public Guid? RequestedById { get; private set; } // Warehouse user
    public Guid? AssignedInspectorId { get; private set; }
    public InspectionRequestStatus Status { get; private set; }
    public DateTime DateCreated { get; private set; }
    public virtual Employee? Employee { get; private set; }
    public virtual Purchase? Purchase { get; private set; }

    private InspectionRequest() { }

    private InspectionRequest(Guid id, Guid? purchaseId, Guid? requestedById, Guid? assignedInspectorId)
    {
        Id = id;
        PurchaseId = purchaseId;
        RequestedById = requestedById;
        AssignedInspectorId = assignedInspectorId;

        Status = assignedInspectorId.HasValue
            ? InspectionRequestStatus.Assigned
            : InspectionRequestStatus.Pending;

        DateCreated = DateTime.UtcNow;

        QueueDomainEvent(new InspectionRequestCreated { RequestId = Id });

        if (assignedInspectorId.HasValue)
        {
            QueueDomainEvent(new InspectionRequestAssigned
            {
                RequestId = Id,
                InspectorId = assignedInspectorId.Value
            });
        }
    }

    public static InspectionRequest Create(Guid? purchaseId, Guid? requestedById, Guid? assignedInspectorId = null)
    {
        return new InspectionRequest(Guid.NewGuid(), purchaseId, requestedById, assignedInspectorId);
    }

    public InspectionRequest Update(Guid? purchaseId, Guid? requestedById, Guid? assignedInspectorId, InspectionRequestStatus status)
    {
        bool isUpdated = false;

        if (PurchaseId != purchaseId)
        {
            PurchaseId = purchaseId;
            isUpdated = true;
        }

        if (RequestedById != requestedById)
        {
            RequestedById = requestedById;
            isUpdated = true;
        }

        if (AssignedInspectorId != assignedInspectorId)
        {
            AssignedInspectorId = assignedInspectorId;
            isUpdated = true;
        }

        if (Status != status)
        {
            Status = status;
            isUpdated = true;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new InspectionRequestUpdated { RequestId = Id });
        }

        return this;
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
        QueueDomainEvent(new InspectionRequestCompleted { RequestId = Id });
    }
}
