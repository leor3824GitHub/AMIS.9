# Developer Quick Reference - Phase 2 Domain Enhancements

## Inventory Management

### Stock Status Management
```csharp
// Mark inventory as quarantined with location
var inventory = await _inventoryRepo.GetByIdAsync(inventoryId);
inventory.MarkAsQuarantined(location: "QUARANTINE-ZONE-A");
await _inventoryRepo.UpdateAsync(inventory);

// Release from quarantine
inventory.ReleaseFromQuarantine();
await _inventoryRepo.UpdateAsync(inventory);

// Mark as damaged
inventory.MarkAsDamaged();

// Mark as obsolete
inventory.MarkAsObsolete();
```

### Reservation Management
```csharp
// Reserve stock for sales order
var inventory = await _inventoryRepo.GetByProductIdAsync(productId);
inventory.ReserveStock(quantity: 50);
await _inventoryRepo.UpdateAsync(inventory);

// Check available quantity
var availableQty = inventory.AvailableQty; // Qty - ReservedQty

// Release reservation when order is cancelled
inventory.ReleaseReservation(quantity: 50);

// Allocate to production
inventory.AllocateToProduction(quantity: 100);
```

### Costing & Location
```csharp
// Set costing method
inventory.SetCostingMethod(CostingMethod.FIFO);

// Set physical location
inventory.SetLocation("Warehouse-A-Aisle-3-Bin-12");

// Record cycle count
inventory.RecordCycleCount(
    countedQty: 485, 
    countDate: DateTime.UtcNow
);
// If variance is negative, status automatically set to UnderCount
```

---

## Purchase Order Workflow

### Approval Workflow
```csharp
var purchase = await _purchaseRepo.GetByIdAsync(purchaseId);

// Submit for approval
purchase.SubmitForApproval();
// Status: Draft → PendingApproval

// Approve purchase
purchase.Approve();
// Status: PendingApproval → Approved

// Reject purchase
purchase.Reject(reason: "Budget not approved");
// Status: PendingApproval → Rejected
```

### Execution Workflow
```csharp
// Vendor acknowledges PO
purchase.Acknowledge();
// Status: Approved → Acknowledged

// Vendor starts processing
purchase.MarkInProgress();
// Status: Acknowledged → InProgress

// Vendor ships items
purchase.MarkShipped();
// Status: InProgress → Shipped
```

### Receipt Workflow
```csharp
// Partial receipt
purchase.MarkPartiallyReceived();
// Status: Shipped → PartiallyReceived

// Full receipt (validates all items accepted)
purchase.MarkFullyReceived();
// Status: PartiallyReceived → FullyReceived
// Throws exception if any items not accepted
```

### Financial Workflow
```csharp
// Invoice pending
purchase.MarkPendingInvoice();
// Status: FullyReceived → PendingInvoice

// Invoice received
purchase.MarkInvoiced();
// Status: PendingInvoice → Invoiced

// Payment pending
purchase.MarkPendingPayment();
// Status: Invoiced → PendingPayment

// Close purchase
purchase.MarkClosed();
// Status: PendingPayment → Closed
```

### Exception Handling
```csharp
// Put on hold
purchase.PutOnHold(reason: "Quality issue discovered");
// Any status → OnHold

// Release from hold
purchase.ReleaseFromHold();
// OnHold → Draft (for re-evaluation)

// Cancel purchase
purchase.Cancel(reason: "Duplicate order");
// Any non-closed status → Cancelled
```

---

## Inspection Quality Management

### Scheduling
```csharp
var inspection = await _inspectionRepo.GetByIdAsync(inspectionId);

// Schedule inspection
inspection.Schedule(scheduledDate: DateTime.UtcNow.AddDays(2));
// Validates date is not in past
```

### Approval Workflows
```csharp
// Conditional approval (with deviations)
inspection.ConditionallyApprove(
    conditions: "Accepted with minor scratches, does not affect functionality"
);
// Adds remarks automatically

// Partial approval
inspection.PartiallyApprove(
    partialDetails: "50% of lot approved, remaining 50% pending re-test"
);
```

### Hold & Quarantine
```csharp
// Put on hold
inspection.PutOnHold(reason: "Waiting for lab test results");
// Status: InProgress → OnHold

// Release from hold
inspection.ReleaseFromHold();
// Status: OnHold → InProgress

// Quarantine materials
inspection.Quarantine(reason: "Failed dimensional inspection");
// Status: InProgress → Quarantined

// Release from quarantine
inspection.ReleaseFromQuarantine();
// Status: Quarantined → InProgress
```

### Re-Inspection
```csharp
// Require re-inspection
inspection.RequireReInspection(
    reason: "First article failed, supplier to resubmit after corrective action"
);
// Status: Rejected/Approved → ReInspectionRequired
// Can transition back to Scheduled or InProgress
```

---

## Product Unit of Measure

### Creating Products
```csharp
// Old way (string)
// var product = Product.Create(name, description, sku, "kg", imagePath, categoryId);

// New way (enum)
var product = Product.Create(
    name: "Industrial Bolt",
    description: "M10x50 hex bolt",
    sku: 12345,
    unit: UnitOfMeasure.Piece,  // ← Type-safe enum
    imagePath: "/images/bolt.jpg",
    categoryId: categoryGuid
);
```

### Updating Products
```csharp
// Update with new unit
product.Update(
    name: "Industrial Bolt - Updated",
    description: null,
    sku: 12345,
    unit: UnitOfMeasure.Box,  // ← Change to box
    imagePath: null,
    categoryId: null
);
```

### Available Units
```csharp
// Quantity
UnitOfMeasure.Piece      // Default
UnitOfMeasure.Each
UnitOfMeasure.Set
UnitOfMeasure.Pair
UnitOfMeasure.Dozen

// Weight
UnitOfMeasure.Kilogram
UnitOfMeasure.Gram
UnitOfMeasure.Ton
UnitOfMeasure.Pound
UnitOfMeasure.Ounce

// Volume
UnitOfMeasure.Liter
UnitOfMeasure.Milliliter
UnitOfMeasure.Gallon

// Length
UnitOfMeasure.Meter
UnitOfMeasure.Centimeter
UnitOfMeasure.Foot
UnitOfMeasure.Inch

// Packaging
UnitOfMeasure.Box
UnitOfMeasure.Case
UnitOfMeasure.Pallet
UnitOfMeasure.Carton
UnitOfMeasure.Drum

// And 20+ more...
```

---

## Event Handlers Examples

### Auto-Quarantine on Failed Inspection
```csharp
public class InspectionRejectedHandler 
    : INotificationHandler<InspectionRejected>
{
    private readonly IReadRepository<Inventory> _inventoryRepo;

    public async Task Handle(
        InspectionRejected notification, 
        CancellationToken ct)
    {
        // Get inventory for rejected inspection
        var inventory = await _inventoryRepo
            .GetByProductIdAsync(notification.ProductId);

        if (inventory != null)
        {
            // Auto-quarantine
            inventory.MarkAsQuarantined(
                location: "QUARANTINE-ZONE"
            );
            
            await _inventoryRepo.UpdateAsync(inventory, ct);
        }
    }
}
```

### Auto-Progress Purchase on Full Receipt
```csharp
public class AcceptanceCompletedHandler 
    : INotificationHandler<AcceptanceCompleted>
{
    private readonly IReadRepository<Purchase> _purchaseRepo;

    public async Task Handle(
        AcceptanceCompleted notification, 
        CancellationToken ct)
    {
        var purchase = await _purchaseRepo
            .GetByIdAsync(notification.PurchaseId);

        if (purchase.IsFullyAccepted && 
            purchase.Status == PurchaseStatus.PartiallyReceived)
        {
            // Auto-progress workflow
            purchase.MarkFullyReceived();
            await _purchaseRepo.UpdateAsync(purchase, ct);
        }
    }
}
```

### Low Stock Alert
```csharp
public class InventoryUpdatedHandler 
    : INotificationHandler<InventoryUpdated>
{
    private readonly INotificationService _notificationService;

    public async Task Handle(
        InventoryUpdated notification, 
        CancellationToken ct)
    {
        var inventory = notification.Inventory;

        if (inventory.StockStatus == StockStatus.BelowReorderPoint)
        {
            await _notificationService.SendAlertAsync(
                message: $"Product {inventory.ProductId} is below reorder point",
                severity: AlertSeverity.Warning
            );
        }
    }
}
```

---

## Common Patterns

### Workflow Validation Pattern
```csharp
public async Task<Result> ApprovePurchaseAsync(Guid purchaseId)
{
    try
    {
        var purchase = await _purchaseRepo.GetByIdAsync(purchaseId);
        
        purchase.Approve(); // ← Throws if invalid transition
        
        await _purchaseRepo.UpdateAsync(purchase);
        return Result.Success();
    }
    catch (InvalidOperationException ex)
    {
        return Result.Failure(ex.Message);
    }
}
```

### Status Check Before Action
```csharp
// Good: Check status before action
if (inspection.Status == InspectionStatus.OnHold)
{
    inspection.ReleaseFromHold();
}

// Or handle exception
try
{
    inspection.ReleaseFromHold();
}
catch (InvalidOperationException)
{
    // Status not OnHold, handle error
}
```

### Available-to-Promise (ATP) Calculation
```csharp
var inventory = await _inventoryRepo.GetByProductIdAsync(productId);

var availableQty = inventory.AvailableQty; // Qty - ReservedQty

if (availableQty >= orderQty)
{
    inventory.ReserveStock(orderQty);
    await _inventoryRepo.UpdateAsync(inventory);
    return Result.Success();
}
else
{
    return Result.Failure($"Only {availableQty} units available");
}
```

---

## Testing Examples

### Unit Test - Inventory Reservation
```csharp
[Fact]
public void ReserveStock_ShouldUpdateReservedQty_AndChangeStatus()
{
    // Arrange
    var inventory = Inventory.Create(
        productId: Guid.NewGuid(),
        qty: 100,
        purchasePrice: 10.00m
    );

    // Act
    inventory.ReserveStock(qty: 50);

    // Assert
    inventory.ReservedQty.Should().Be(50);
    inventory.AvailableQty.Should().Be(50);
    inventory.StockStatus.Should().Be(StockStatus.Reserved);
}
```

### Unit Test - Purchase Workflow
```csharp
[Fact]
public void Approve_ShouldThrow_WhenNotPendingApproval()
{
    // Arrange
    var purchase = Purchase.Create(
        supplierId: Guid.NewGuid(),
        purchaseDate: DateTime.UtcNow,
        totalAmount: 1000m,
        status: PurchaseStatus.Draft
    );

    // Act & Assert
    var act = () => purchase.Approve();
    
    act.Should().Throw<InvalidOperationException>()
        .WithMessage("Only pending purchases can be approved.");
}
```

### Integration Test - Inspection State Machine
```csharp
[Fact]
public async Task CompleteInspectionWorkflow_ShouldTransitionThroughAllStates()
{
    // Arrange
    var inspection = Inspection.Create(
        inspectionRequestId: Guid.NewGuid(),
        employeeId: Guid.NewGuid()
    );

    // Act - Schedule
    inspection.Schedule(DateTime.UtcNow.AddDays(1));
    inspection.Status.Should().Be(InspectionStatus.Scheduled);

    // Act - Start
    inspection.ChangeStatus(InspectionStatus.InProgress);

    // Act - Complete
    inspection.Complete();
    inspection.Status.Should().Be(InspectionStatus.Completed);

    // Act - Approve
    inspection.Approve(purchaseId: Guid.NewGuid());
    inspection.Status.Should().Be(InspectionStatus.Approved);

    // Assert
    inspection.Approved.Should().BeTrue();
}
```

---

## API Examples

### Inventory Reserve Endpoint
```csharp
public class ReserveInventoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/inventory/{id}/reserve", 
            async (Guid id, ReserveInventoryCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request with { InventoryId = id });
            return Results.Ok(response);
        })
        .WithName(nameof(ReserveInventoryEndpoint))
        .RequirePermission("Permissions.Inventory.Reserve")
        .MapToApiVersion(new ApiVersion(1, 0));
    }
}
```

### Purchase Workflow Endpoints
```csharp
// Approve
app.MapPost("/api/purchases/{id}/approve", 
    async (Guid id, ISender mediator) => 
{
    await mediator.Send(new ApprovePurchaseCommand { PurchaseId = id });
    return Results.NoContent();
});

// Ship
app.MapPost("/api/purchases/{id}/ship", 
    async (Guid id, ISender mediator) => 
{
    await mediator.Send(new MarkPurchaseShippedCommand { PurchaseId = id });
    return Results.NoContent();
});
```

---

## Migration Checklist

When updating existing commands/handlers:

### ✅ Product Commands
- [ ] Update `CreateProductCommand.Unit` from `string` to `UnitOfMeasure`
- [ ] Update `UpdateProductCommand.Unit` from `string?` to `UnitOfMeasure?`
- [ ] Update validators to check enum range
- [ ] Update DTOs/responses to return enum or string

### ✅ Inventory Commands (New)
- [ ] Create `ReserveInventoryCommand`
- [ ] Create `ReleaseReservationCommand`
- [ ] Create `QuarantineInventoryCommand`
- [ ] Create `RecordCycleCountCommand`

### ✅ Purchase Commands (Enhanced)
- [ ] Create `ApprovePurchaseCommand`
- [ ] Create `MarkPurchaseShippedCommand`
- [ ] Create `InvoicePurchaseCommand`

### ✅ Inspection Commands (Enhanced)
- [ ] Create `ScheduleInspectionCommand`
- [ ] Create `ConditionallyApproveInspectionCommand`
- [ ] Create `QuarantineInspectionCommand`

### ✅ Tests
- [ ] Update Product tests for enum
- [ ] Add Inventory workflow tests
- [ ] Add Purchase workflow tests
- [ ] Add Inspection state machine tests

---

**Quick Reference Version**: 2.0  
**Last Updated**: November 2025  
**Related Docs**: PHASE2_DOMAIN_ENHANCEMENTS.md, VALUE_OBJECTS_ENHANCEMENT.md
