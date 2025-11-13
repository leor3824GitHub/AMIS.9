# Aggregate Usage Quick Reference

## Purchase Aggregate

### Creating a Purchase
```csharp
// Create new purchase
var purchase = Purchase.Create(
    supplierId: supplierGuid,
    purchaseDate: DateTime.UtcNow,
    referenceNumber: "PO-2024-001",
    remarks: "Bulk order"
);

// Add items
purchase.AddItem(productId, qty: 100, unitPrice: 25.50m, itemStatus: PurchaseStatus.Draft);

// TotalAmount is calculated automatically
```

### Purchase Workflow
```csharp
// 1. Submit for processing
purchase.Submit();

// 2. Mark as delivered (partial or full)
purchase.MarkAsPartiallyDelivered();
purchase.MarkAsDelivered();

// 3. Close after inspection and acceptance
purchase.Close(); // Validates full inspection and acceptance

// Alternative: Cancel if needed
purchase.Cancel("Supplier cannot fulfill order");
```

### Updating Purchase
```csharp
purchase.Update(
    supplierId: newSupplierGuid,
    purchaseDate: updatedDate,
    referenceNumber: "PO-2024-001-REV",
    remarks: "Updated remarks"
);
// Note: TotalAmount and Status cannot be updated directly; use workflow methods.
```

### Managing Items
```csharp
// Update item (status here is ItemStatus - not Purchase aggregate Status)
purchase.UpdateItem(itemId, productId, qty: 150, unitPrice: 24.00m, PurchaseStatus.Draft);

// Remove item
purchase.RemoveItem(itemId);
```

## Inspection Aggregate

### Creating an Inspection
```csharp
var inspection = Inspection.Create(
    purchaseId: purchaseGuid,
    employeeId: inspectorGuid,
    inspectedOn: DateTime.UtcNow,
    remarks: "Quality check",
    iarDocumentPath: "/docs/iar-001.pdf"
);
```

### Adding Inspection Items
```csharp
// Add inspection results
inspection.AddItem(
    purchaseItemId: itemGuid,
    qtyInspected: 100,
    qtyPassed: 95,
    qtyFailed: 5,
    remarks: "Minor defects found",
    inspectionItemStatus: InspectionItemStatus.Partial
);

// Quantities must validate: qtyPassed + qtyFailed = qtyInspected
// Duplicate inspection items for the same purchase item are not allowed.
```

### Inspection Workflow
```csharp
// 1. Complete inspection (when all items inspected)
inspection.Complete();

// 2. Approve inspection
inspection.Approve(); // Requires at least one passed item and Completed status

// Alternative paths:
inspection.Reject("Quality standards not met");
inspection.Cancel("Inspector unavailable");
```

### Auto-evaluation
```csharp
// Automatically determine if inspection is complete
inspection.EvaluateAndSetStatus(purchase);
```

### Querying Inspection Results
```csharp
var acceptedItems = inspection.AcceptedItems();
var rejectedItems = inspection.RejectedItems();
var totalPassed = inspection.TotalPassedQuantity;
var hasFailures = inspection.HasAnyFailed;
```

## Acceptance Aggregate

### Creating an Acceptance
```csharp
var acceptance = Acceptance.Create(
    purchaseId: purchaseGuid,
    supplyOfficerId: officerGuid,
    acceptanceDate: DateTime.UtcNow,
    remarks: "Received in good condition",
    inspectionId: inspectionGuid // Optional but recommended
);
```

### Adding Acceptance Items
```csharp
acceptance.AddItem(
    purchaseItemId: itemGuid,
    qtyAccepted: 95, // Cannot exceed qtyPassed from inspection
    remarks: "5 items rejected due to defects"
);
// Each purchase item can only appear once per acceptance.
```

### Validation Against Inspection
```csharp
// Validate that acceptance doesn't exceed inspection results
acceptance.ValidateAgainstInspection(inspection); // Inspection must be Approved
```

### Posting Acceptance
```csharp
// Post acceptance (final step, cannot modify after)
acceptance.PostAcceptance();

// Check status
if (acceptance.IsPosted)
{
    // Acceptance is finalized
}
```

### Acceptance Properties
```csharp
var isFullAcceptance = acceptance.IsFullAcceptance; // All items fully accepted
var isPartialAcceptance = acceptance.IsPartialAcceptance; // Some items partial
var totalAccepted = acceptance.TotalAcceptedQuantity;
```

## Common Patterns

### Complete Workflow Example
```csharp
// 1. Create Purchase
var purchase = Purchase.Create(supplierGuid, DateTime.UtcNow);
purchase.AddItem(productGuid, 100, 25.00m);
purchase.Submit();
await purchaseRepo.AddAsync(purchase);

// 2. Mark as Delivered
purchase.MarkAsDelivered();
await purchaseRepo.UpdateAsync(purchase);

// 3. Create Inspection
var inspection = Inspection.Create(purchase.Id, inspectorGuid);
inspection.AddItem(purchaseItemId, 100, 95, 5, "Minor defects");
inspection.Complete();
inspection.Approve();
await inspectionRepo.AddAsync(inspection);

// 4. Create Acceptance
var acceptance = Acceptance.Create(
    purchase.Id, 
    officerGuid, 
    DateTime.UtcNow, 
    "Accepted with minor defects",
    inspection.Id
);
acceptance.AddItem(purchaseItemId, 95, "5 rejected");
acceptance.ValidateAgainstInspection(inspection);
acceptance.PostAcceptance();
await acceptanceRepo.AddAsync(acceptance);

// 5. Close Purchase (auto-close may also occur in handlers)
purchase.Close();
await purchaseRepo.UpdateAsync(purchase);
```

### Handling Partial Acceptance
```csharp
// First acceptance
var acceptance1 = Acceptance.Create(purchaseId, officerGuid, DateTime.UtcNow, "Batch 1");
acceptance1.AddItem(purchaseItemId, 50); // Accept 50 of 100
acceptance1.PostAcceptance();

// Later, create second acceptance for remaining items
var acceptance2 = Acceptance.Create(purchaseId, officerGuid, DateTime.UtcNow, "Batch 2");
acceptance2.AddItem(purchaseItemId, 45); // Accept 45 more
acceptance2.PostAcceptance();

// Check purchase status
if (!purchase.IsFullyAccepted)
{
    // Still 5 items not accepted
}
```

### Status Transitions

#### Purchase Status Flow
```
Draft ? Submitted ? PartiallyDelivered ? Delivered ? Closed
   ?         ?              ?
Cancelled ?????????????????????
```

#### Inspection Status Flow
```
InProgress ? Completed ? Approved
    ?            ?
Rejected ?????????
    ?
Cancelled
```

#### Acceptance Status Flow
```
Pending ? Posted
   ?
Cancelled
```

## Validation Rules Summary

### Purchase
- Cannot modify closed or cancelled purchases
- Cannot close without full inspection and acceptance
- Cannot cancel if partially accepted
- Must have items to submit

### Inspection
- Cannot add/remove items unless InProgress
- Cannot approve without items
- Cannot approve unless at least one item passed and Completed
- Must complete before approval
- Passed + Failed must equal Inspected
- No duplicate purchase item lines

### Acceptance
- Cannot modify after posting
- Cannot accept more than inspected/passed quantity
- Cannot post without items
- Acceptance date cannot be in future
- Must validate against inspection if linked
- No duplicate purchase item lines

## Computed Properties

### Purchase
```csharp
purchase.TotalAmount        // Auto-calculated from items
purchase.HasItems          // Any items exist?
purchase.TotalItemsCount   // Sum of all quantities
purchase.IsFullyInspected  // All items inspected?
purchase.IsFullyAccepted   // All items accepted?
```

### PurchaseItem
```csharp
item.TotalPrice           // Qty * UnitPrice
item.QtyRemaining         // Qty - QtyAccepted
item.IsFullyInspected     // QtyInspected >= Qty
item.IsFullyAccepted      // QtyAccepted >= Qty
item.HasFailedInspection  // QtyFailed > 0
```

### Inspection
```csharp
inspection.HasItems
inspection.TotalInspectedQuantity
inspection.TotalPassedQuantity
inspection.TotalFailedQuantity
inspection.HasAnyPassed
inspection.HasAnyFailed
```

### InspectionItem
```csharp
item.PassRate          // (QtyPassed / QtyInspected) * 100
item.IsFullyPassed     // No failures
item.IsFullyFailed     // No passes
```

### Acceptance
```csharp
acceptance.HasItems
acceptance.TotalAcceptedQuantity
acceptance.IsFullAcceptance    // All items fully accepted
acceptance.IsPartialAcceptance // Some items partial
```

### AcceptanceItem
```csharp
item.IsFullyAccepted      // Accepted >= Purchase Qty
item.IsPartiallyAccepted  // 0 < Accepted < Purchase Qty
item.QtyRemaining         // Purchase Qty - Accepted
```

## Common Exceptions Thrown
```csharp
InvalidOperationException // Invalid workflow/state transition or duplicate entity
ArgumentException         // Invalid input (negative qty, future date, etc.)
```

// Handlers automatically adjust Purchase status based on inspection & acceptance progress.
