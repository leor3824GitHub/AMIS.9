using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;
using AMIS.WebApi.Catalog.Domain.Events;
using AMIS.WebApi.Catalog.Domain.ValueObjects;

namespace AMIS.WebApi.Catalog.Domain;

public class InspectionRequest : AuditableEntity, IAggregateRoot
{
    public Guid? PurchaseId { get; private set; }
    //public Guid? RequestedById { get; private set; } // Warehouse user
    public Guid? InspectorId { get; private set; }
    public InspectionRequestStatus Status { get; private set; }
    public DateTime DateCreated { get; private set; }
    public virtual Employee? Inspector { get; private set; }
    public virtual Purchase? Purchase { get; private set; }

    private InspectionRequest() { }

    private InspectionRequest(Guid id, Guid? purchaseId, Guid? inspectorId)
    {
        Id = id;
        PurchaseId = purchaseId;
        //RequestedById = requestedById;
        InspectorId = inspectorId;

        Status = inspectorId.HasValue
            ? InspectionRequestStatus.Assigned
            : InspectionRequestStatus.Pending;

        DateCreated = DateTime.UtcNow;

        QueueDomainEvent(new InspectionRequestCreated { RequestId = Id });

        if (inspectorId.HasValue)
        {
            QueueDomainEvent(new InspectionRequestAssigned
            {
                RequestId = Id,
                InspectorId = inspectorId.Value
            });
        }
    }

    public static InspectionRequest Create(Guid? purchaseId, Guid? inspectorId = null)
    {
        return new InspectionRequest(Guid.NewGuid(), purchaseId, inspectorId);
    }

    public InspectionRequest Update(Guid? purchaseId, Guid? inspectorId)
    {
        bool isUpdated = false;

        if (PurchaseId != purchaseId)
        {
            PurchaseId = purchaseId;
            isUpdated = true;
        }

        if (InspectorId != inspectorId)
        {
            InspectorId = inspectorId;
            isUpdated = true;

            Status = inspectorId.HasValue
            ? InspectionRequestStatus.Assigned
            : InspectionRequestStatus.Pending;
        }

        if (isUpdated)
        {
            QueueDomainEvent(new InspectionRequestUpdated { RequestId = Id });
        }

        return this;
    }

    public void AssignInspector(Guid inspectorId)
    {
        InspectorId = inspectorId;
        Status = InspectionRequestStatus.Assigned;
        QueueDomainEvent(new InspectionRequestAssigned { RequestId = Id, InspectorId = inspectorId });
    }

    public void MarkCompleted()
    {
        Status = InspectionRequestStatus.Completed;
        QueueDomainEvent(new InspectionRequestCompleted { RequestId = Id });
    }
}
