# Data Model: Purchase Request Item Product Input

## Updated Entity: PurchaseRequestItem

**Context**: `api/modules/Catalog/Catalog.Domain/PurchaseRequestItem.cs`

### Fields (conceptual)

- `Id: Guid`
- `PurchaseRequestId: Guid`
- `ProductId: Guid?`
- `ManualProductName: string?`
- `Qty: int`
- `Unit: string`
- `Description: string?`

### Invariants / Validation Rules

- Exactly one product source MUST be set:
  - `ProductId != null XOR !string.IsNullOrWhiteSpace(ManualProductName)`
- `Qty > 0`
- `Unit` is required and bounded (max length per existing validator constraints)
- `Description` remains optional and bounded

### State transitions

- This feature does not introduce new workflow states; it only changes how PR item identity is captured.

## DTO/Contract Updates

### CreatePurchaseRequestCommand.PurchaseRequestItemCreateDto

Add:
- `ManualProductName: string?`

Rules:
- Same XOR rule between `ProductId` and `ManualProductName`

### PurchaseRequestItemResponse

Add:
- `ManualProductName: string?`

Purpose:
- Enables the UI to display product label when `ProductId` is null.

### Add/Update Purchase Request Item Commands

Add:
- `ManualProductName: string?`

Rules:
- Same XOR rule

## Persistence

- Add a new nullable column to the PurchaseRequestItem table, e.g. `ManualProductName` (varchar, length bounded).
- Update EF Core configuration for `PurchaseRequestItem` accordingly.

## Auditability

- Existing `AuditableEntity` timestamps/users plus existing domain events (`PurchaseRequestItemCreated`, `PurchaseRequestItemUpdated`) should fire when manual name changes.
