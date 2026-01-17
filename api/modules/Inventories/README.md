# Catalog Module — Domain Summary

Purpose
- Domain layer for product/catalog/inventory management used by AMIS (Catalog feature set).
- Implements business rules for inventory, transactions, purchases, issuances, inspections, acceptances and physical assets.

Key Entities (aggregate roots)
- `Inventory` — holds `ProductId`, `Qty`, `AvePrice`. Business logic: `AddStock`, `UpdateStock`, `DeductStock`.
- `InventoryTransaction` — transactional record of receipts/issuances with `Qty`, `UnitCost`, `TransactionType`, `Location`, `SourceId`.
- `Product` — product master data (SKU/brand/category, unit of measure, etc.).
- `Issuance` / `IssuanceItem` — issuance documents and items.
- `Purchase` / `PurchaseItem` — purchase order and items.
- `PhysicalAsset` — long‑lived asset details and reclassification history.
- `Acceptance` / `AcceptanceItem` — acceptance records for deliveries/receipts.

Value Objects / Domain Types
- `TransactionType`, `PropertyClassification`, `AcceptanceStatus`, `InspectionStatus`, `RCAAccountCode`, etc. (see `ValueObjects/`).

Domain Events
- Examples: `InventoryCreated`, `InventoryUpdated`, `InventoryTransactionCreated`, `InventoryTransactionUpdated`, `ProductCreated`, `IssuanceCreated`, `PurchaseCreated`, `InspectionCreated`, `AcceptancePosted`, etc. Handlers usually implement `INotificationHandler<T>` and perform side effects (caching, metrics, downstream processes).

Core Invariants & Rules
- Weighted Average Price: `Inventory.AddStock` and `UpdateStock` update `AvePrice` correctly: AvePrice = ((AvePrice * Qty) + (unitPrice * addedQty)) / (Qty + addedQty).
- Quantity rules: quantities must be >= 0 and operations that would make Qty negative throw `InvalidOperationException`.
- Stock validation: purchase price and qty must be > 0.
- Domain events are queued via `QueueDomainEvent(...)` on aggregate modifications.

Auditing & Traceability
- Entities derive from `AuditableEntity` and include `Created`, `CreatedBy`, `LastModified`, `Deleted`, etc.
- Event records include timestamps and are dispatched through MediatR (`DomainEvent : INotification`).

Migrations & Database
- Migrations project: `api/migrations/PostgreSQL` (Catalog folder inside). Use documented EF Core commands from repo root to add or apply migrations targeting `CatalogDbContext` (see root `README.md`).

Suggested Tests
- Unit tests for avg price calculations (AddStock, UpdateStock, DeductStock with/without unit price).
- Tests for validation rules (negative qty, zero price, insufficient stock errors).
- Integration test exercising event dispatch (queue domain event → registered handler runs) and DB transactions (using in-memory or test Postgres).

Developer Notes / Examples
- Create inventory:
```
var inv = Inventory.Create(productId, qty: 10, purchasePrice: 100.00m);
```
- Add stock (updates average price):
```
inv.AddStock(5, 120.00m);
```
- Deduct stock:
```
inv.DeductStock(3); // or DeductStock(3, unitPrice)
```

Event Handler Coverage
- Many events exist under `Events/`. Ensure any event with required side effects has a corresponding `INotificationHandler<T>` implementation.

Contributing / Best Practices
- Keep aggregate logic inside entities; use application layer for orchestration and workflows.
- Create migrations per module and keep them descriptive.
- Add module-level tests and sample fixture data for common scenarios.

Location
- Domain sources: `api/modules/Catalog/Catalog.Domain/`
- Migrations: `api/migrations/PostgreSQL/Catalog/`

If you want, I can add unit tests for `Inventory` avg price behavior and a short list of missing event handlers (events without handlers).