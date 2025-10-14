using System;
using AMIS.Framework.Core.Domain;
using AMIS.Framework.Core.Domain.Contracts;

namespace AMIS.WebApi.Catalog.Domain;
public class InspectionItem : AuditableEntity, IAggregateRoot
{
    public Guid InspectionId { get; set; }
    public virtual Inspection Inspection { get; set; } = default!;
    public Guid PurchaseItemId { get; set; }
    public virtual PurchaseItem PurchaseItem { get; set; } = default!;
    public int InspectedQty { get; set; }
    public bool Accepted { get; set; }
    public string? Notes { get; set; }

    // Parameterless constructor for EF / serializers
    private InspectionItem() { }

    // Internal constructor that sets all fields including Id
    private InspectionItem(Guid inspectionId, Guid purchaseItemId, int inspectedQty, bool accepted, string? notes)
    {
        InspectionId = inspectionId;
        PurchaseItemId = purchaseItemId;
        InspectedQty = inspectedQty;
        Accepted = accepted;
        Notes = notes;
    }

    // Factory method to create a new InspectionItem
    public static InspectionItem Create(Guid inspectionId, Guid purchaseItemId, int inspectedQty, bool accepted, string? notes)
    {
        if (inspectedQty < 0) throw new ArgumentException("Inspected quantity cannot be negative.", nameof(inspectedQty));
        return new InspectionItem(inspectionId, purchaseItemId, inspectedQty, accepted, notes);
    }

    // Domain method: update fields if provided (null means no change)
    public InspectionItem Update(Guid? purchaseItemId = null, int? inspectedQty = null, bool? accepted = null, string? notes = null)
    {
        bool isUpdated = false;

        if (purchaseItemId.HasValue && PurchaseItemId != purchaseItemId.Value)
        {
            PurchaseItemId = purchaseItemId.Value;
            isUpdated = true;
        }

        if (inspectedQty.HasValue)
        {
            if (inspectedQty.Value < 0) throw new ArgumentException("Inspected quantity cannot be negative.", nameof(inspectedQty));
            if (InspectedQty != inspectedQty.Value)
            {
                InspectedQty = inspectedQty.Value;
                isUpdated = true;
            }
        }

        if (accepted.HasValue && Accepted != accepted.Value)
        {
            Accepted = accepted.Value;
            isUpdated = true;
        }

        if (notes is not null)
        {
            // Treat empty or whitespace as explicit update to notes (could clear)
            if (!string.Equals(Notes, notes, StringComparison.Ordinal))
            {
                Notes = notes;
                isUpdated = true;
            }
        }

        // If needed, queue domain events here (e.g., InspectionItemUpdated) - not added to avoid referencing missing types.
        return this;
    }

    // Domain method: mark this inspection item as accepted
    public InspectionItem Accept()
    {
        // Business rule: inspected quantity must be non-negative (should already be validated elsewhere)
        if (InspectedQty < 0) throw new InvalidOperationException("Cannot accept an item with negative inspected quantity.");
        if (!Accepted)
        {
            Accepted = true;
            // Optionally queue domain event here
        }
        return this;
    }

    // Domain method: mark this inspection item as rejected
    public InspectionItem Reject()
    {
        if (Accepted)
        {
            Accepted = false;
            // Optionally queue domain event here
        }
        return this;
    }

    // Domain method: set inspected quantity with validation
    public InspectionItem SetInspectedQuantity(int qty)
    {
        if (qty < 0) throw new ArgumentException("Inspected quantity cannot be negative.", nameof(qty));
        if (InspectedQty != qty)
        {
            InspectedQty = qty;
            // Optionally queue domain event here
        }
        return this;
    }

    // Domain method: add or update notes. Passing null or whitespace clears the notes.
    public InspectionItem AddOrUpdateNotes(string? notes)
    {
        var newNotes = string.IsNullOrWhiteSpace(notes) ? null : notes;
        if (!string.Equals(Notes, newNotes, StringComparison.Ordinal))
        {
            Notes = newNotes;
            // Optionally queue domain event here
        }
        return this;
    }

    // add domain methods as needed
}
