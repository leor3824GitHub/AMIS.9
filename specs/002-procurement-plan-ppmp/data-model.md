# Data Model: Procurement Plan (PPMP)

This model follows the AMIS constitution section “Domain Entities: Procurement Plan (PPMP)”.

## Aggregate 1: `ProcurementPlan`

### Root: `ProcurementPlanHeader`

Core fields:
- `Id : Guid`
- `ControlNumber : string` (unique per tenant)
- `FiscalYear : int`
- `DepartmentId : Guid`
- `DepartmentName : string` (read-only/denormalized)
- `Status : ProcurementPlanStatus` (`DRAFT`, `PENDING_APPROVAL`, `APPROVED`)
- `IsSupplemental : bool`
- `BudgetType : BudgetType` (`INDICATIVE`, `FINAL`)
- `TotalBudget : decimal` (computed)
- `PreparedByUserId : Guid`
- `SubmissionDate : DateTimeOffset?`
- `ApprovedByUserId : Guid?`
- `ApprovalDate : DateTimeOffset?`

Invariants:
- `ControlNumber` is required and unique within tenant.
- Only `DRAFT` plans can be edited (except explicitly allowed supplemental workflow).
- `SubmissionDate` set when transitioning to `PENDING_APPROVAL`.
- `ApprovalDate` set when transitioning to `APPROVED`.
- `TotalBudget = sum(Items.EstimatedBudget)`.

### Owned: `ProcurementPlanItem`

Core fields:
- `Id : Guid`
- `PlanHeaderId : Guid` (FK)
- `PapCode : string?`
- `Description : string` (required)
- `ProjectType : ProjectType` (`GOODS`, `INFRASTRUCTURE`, `CONSULTING_SERVICES`)
- `Quantity : int` (required, > 0)
- `UnitOfMeasure : string` (required)
- `UnitCost : decimal` (required, >= 0)
- `EstimatedBudget : decimal` (computed = `Quantity * UnitCost`)
- `Mode : ModeOfProcurement` (enum or lookup)
- `IsEarlyProcurement : bool`
- `ScheduleMonth : string` (required; consider enum)
- `FundingSource : string` (required)
- `Remarks : string?`

Invariants:
- Estimated budget must always recompute on quantity/unit cost change.
- Item changes are blocked when header is `APPROVED`.

## Aggregate 2: `ProcurementProject`

### Root: `ProcurementProject`

Core fields:
- `ProjectId : Guid`
- `PapCode : string?`
- `ProjectTitle : string` (required)
- `PmoEndUser : string` (required)
- `IsEPA : bool`
- `ModeId : int` (FK to mode lookup)
- `FundSource : string` (required)
- `Remarks : string?`

### Owned: `ProcurementSchedule`

Core fields:
- `ScheduleId : Guid`
- `ProjectId : Guid`
- `AdsPosting : DateOnly?`
- `BidOpening : DateOnly?`
- `NoticeOfAward : DateOnly?`
- `ContractSigning : DateOnly?`

### Owned: `ProjectBudget`

Core fields:
- `BudgetId : Guid`
- `ProjectId : Guid`
- `TotalAmount : decimal(15,2)`
- `MooeAmount : decimal(15,2)`
- `CoAmount : decimal(15,2)`

Invariants:
- `TotalAmount = MooeAmount + CoAmount`.

## Cross-cutting fields (tenant/audit)

All persisted entities follow framework conventions for:
- `TenantId` (Finbuckle isolation)
- audit fields (`Created`, `CreatedBy`, `LastModified`, `LastModifiedBy`) as used by the base `FshDbContext`.
