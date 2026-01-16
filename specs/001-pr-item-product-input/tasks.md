---
description: "Task list for feature implementation"
---

# Tasks: Purchase Request Item Product Input

**Input**: Design documents from `/specs/001-pr-item-product-input/` (plan.md, spec.md, data-model.md, research.md, contracts/openapi.yaml, quickstart.md)

**Tests**: Included for domain invariants + FluentValidation rules (test-first where practical).

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (US1, US2, US3)
- All task descriptions include exact file paths

---

## Phase 1: Setup (Shared Infrastructure)

- [x] T001 Review scope + acceptance scenarios in specs/001-pr-item-product-input/spec.md
- [x] T002 Review implementation constraints and file targets in specs/001-pr-item-product-input/plan.md
- [x] T003 Validate contracts excerpt and payload expectations in specs/001-pr-item-product-input/contracts/openapi.yaml
- [x] T004 Run baseline build to ensure a clean starting point in AMIS.9.sln

---

## Phase 2: Foundational (Blocking Prerequisites)

**⚠️ CRITICAL**: Complete this phase before any user story work.

- [ ] T005 Confirm XOR invariant + normalization in api/modules/Catalog/Catalog.Domain/PurchaseRequestItem.cs
- [ ] T006 Confirm aggregate methods accept manual name in api/modules/Catalog/Catalog.Domain/PurchaseRequest.cs
- [ ] T007 Confirm EF mapping for ManualProductName length in api/modules/Catalog/Catalog.Infrastructure/Persistence/Configurations/PurchaseRequestItemConfiguration.cs
- [ ] T008 Confirm Postgres migration + snapshot include ManualProductName in api/migrations/PostgreSQL/Catalog/20251224152848_AddPurchaseRequestItemManualProductName.cs and api/migrations/PostgreSQL/Catalog/CatalogDbContextModelSnapshot.cs
- [ ] T009 Confirm create DTO includes ManualProductName in api/modules/Catalog/Catalog.Application/PurchaseRequests/Create/v1/CreatePurchaseRequestCommand.cs
- [ ] T010 Confirm create validator enforces XOR + max lengths in api/modules/Catalog/Catalog.Application/PurchaseRequests/Create/v1/CreatePurchaseRequestCommandValidator.cs
- [ ] T011 Add FluentValidation for item add/update XOR rule in api/modules/Catalog/Catalog.Application/PurchaseRequests/ManageItems/v1/AddPurchaseRequestItemCommandValidator.cs
- [ ] T012 Add FluentValidation for item add/update XOR rule in api/modules/Catalog/Catalog.Application/PurchaseRequests/ManageItems/v1/UpdatePurchaseRequestItemCommandValidator.cs
- [ ] T013 Ensure validators are discovered via assembly scanning in api/server/Extensions.cs

**Checkpoint**: Foundation ready; user stories can proceed.

---

## Phase 3: User Story 1 - Add PR item by selecting an existing product (Priority: P1) 🎯 MVP

**Goal**: Users can pick a product from the product library for each PR item.

**Independent Test**: Create a PR item in “Select from product list” mode, verify ProductId is stored and the UI shows the selected product name.

### Tests for User Story 1 (write first; expect failing until implementation)

- [ ] T014 [P] [US1] Add validator tests for product selection path in TestProject.XUnit/PurchaseRequestItemProductSelectionValidatorTests.cs
- [ ] T015 [P] [US1] Add domain tests for product selection invariant in TestProject.XUnit/PurchaseRequestItemProductSelectionDomainTests.cs

### Implementation for User Story 1

- [ ] T016 [US1] Ensure CreatePurchaseRequestHandler passes ProductId when provided in api/modules/Catalog/Catalog.Application/PurchaseRequests/Create/v1/CreatePurchaseRequestHandler.cs
- [x] T017 [US1] Ensure product selection is tenant-scoped via SearchProducts endpoint usage in apps/blazor/client/Pages/Catalog/PurchaseRequests/PurchaseRequestDialog.razor
- [x] T018 [US1] Update product selector to be searchable (or minimal scrolling-safe) in apps/blazor/client/Pages/Catalog/PurchaseRequests/PurchaseRequestItemList.razor
- [x] T019 [US1] Display selected product name in create/edit rows in apps/blazor/client/Pages/Catalog/PurchaseRequests/PurchaseRequestItemList.razor

---

## Phase 4: User Story 2 - Add PR item by manual product entry (Priority: P2)

**Goal**: Users can type the product name for PR items not yet in the library.

**Independent Test**: Create a PR item in “Manual entry” mode, verify ManualProductName is stored and displayed as entered.

### Tests for User Story 2 (write first; expect failing until implementation)

- [ ] T020 [P] [US2] Add create-command validator tests for manual entry rules in TestProject.XUnit/PurchaseRequestItemManualEntryValidatorTests.cs
- [ ] T021 [P] [US2] Add domain tests for trimming + max length expectations in TestProject.XUnit/PurchaseRequestItemManualEntryDomainTests.cs

### Implementation for User Story 2

- [x] T022 [US2] Add Manual entry mode UI + bind ManualProductName in apps/blazor/client/Pages/Catalog/PurchaseRequests/PurchaseRequestItemList.razor
- [x] T023 [US2] Update AddItem flow to allow ManualProductName and no ProductId in apps/blazor/client/Pages/Catalog/PurchaseRequests/PurchaseRequestItemList.razor
- [x] T024 [US2] Update edit flow to allow editing ManualProductName in apps/blazor/client/Pages/Catalog/PurchaseRequests/PurchaseRequestItemList.razor
- [ ] T025 [US2] Update CreatePurchaseRequestHandler to pass ManualProductName when provided in api/modules/Catalog/Catalog.Application/PurchaseRequests/Create/v1/CreatePurchaseRequestHandler.cs
- [x] T026 [US2] Update PR view table to show manual name and label it as manual in apps/blazor/client/Pages/Catalog/PurchaseRequests/PurchaseRequestDialog.razor

---

## Phase 5: User Story 3 - Prevent inconsistent product input (Priority: P3)

**Goal**: Prevent saving PR items with both ProductId and ManualProductName (or neither).

**Independent Test**: Switching modes clears the opposite field; invalid combinations are rejected client-side and server-side.

### Tests for User Story 3 (write first; expect failing until implementation)

- [ ] T027 [P] [US3] Add XOR invariant tests (both/neither invalid) in TestProject.XUnit/PurchaseRequestItemXorInvariantTests.cs
- [ ] T028 [P] [US3] Add ManageItems validator tests for XOR + required fields in TestProject.XUnit/PurchaseRequestItemManageItemsValidatorTests.cs

### Implementation for User Story 3

- [x] T029 [US3] Implement mode switching that clears the other field (ProductId ↔ ManualProductName) in apps/blazor/client/Pages/Catalog/PurchaseRequests/PurchaseRequestItemList.razor
- [x] T030 [US3] Add client-side validation for manual name required / product required before adding or saving edits in apps/blazor/client/Pages/Catalog/PurchaseRequests/PurchaseRequestItemList.razor
- [ ] T031 [US3] Ensure add/update item endpoints validate XOR via new validators in api/modules/Catalog/Catalog.Application/PurchaseRequests/ManageItems/v1/AddPurchaseRequestItemCommandValidator.cs and api/modules/Catalog/Catalog.Application/PurchaseRequests/ManageItems/v1/UpdatePurchaseRequestItemCommandValidator.cs
- [ ] T032 [US3] Ensure auditability on product source change by verifying domain events fire on source changes in api/modules/Catalog/Catalog.Domain/PurchaseRequestItem.cs

---

## Phase 6: Polish & Cross-Cutting Concerns

- [ ] T033 [P] Populate Product details in purchase request retrieval response in api/modules/Catalog/Catalog.Application/PurchaseRequests/Get/v1/GetPurchaseRequestHandler.cs
- [x] T034 [P] Update PR view to display Product.Name when available (else ManualProductName) in apps/blazor/client/Pages/Catalog/PurchaseRequests/PurchaseRequestDialog.razor
- [ ] T035 Regenerate NSwag client so manualProductName flows into generated API models in AMIS.nswag and apps/blazor/infrastructure/Api/ApiClient.cs
- [ ] T036 Run quickstart verification steps and update if needed in specs/001-pr-item-product-input/quickstart.md
- [ ] T037 Run tests for the feature changes in TestProject.XUnit/TestProject.XUnit.csproj
- [ ] T038 Run solution build to confirm no regressions in AMIS.9.sln

---

## Dependencies & Execution Order

### User Story Dependencies

- US1 (P1) depends on Phase 2 foundational completion.
- US2 (P2) depends on Phase 2 foundational completion.
- US3 (P3) depends on US1 + US2 UI being present (mode switching between two modes).

### Dependency Graph (stories)

- Phase 1 → Phase 2 → US1
- Phase 1 → Phase 2 → US2
- Phase 1 → Phase 2 → (US1 + US2) → US3

### Parallel Opportunities (examples)

- Phase 2: T011 and T012 can be done in parallel.
- US1 tests: T014 and T015 can be done in parallel.
- US2 tests: T020 and T021 can be done in parallel.
- US3 tests: T027 and T028 can be done in parallel.
- Polish: T033 and T034 can be done in parallel.

---

## Parallel Examples (Per Story)

### US1

```text
Run in parallel:
- T014 [US1] validator tests
- T015 [US1] domain tests
Then:
- T016 → T019 [US1] implementation
```

### US2

```text
Run in parallel:
- T020 [US2] validator tests
- T021 [US2] domain tests
Then:
- T022 → T026 [US2] implementation
```

### US3

```text
Run in parallel:
- T027 [US3] XOR invariant tests
- T028 [US3] manage-items validator tests
Then:
- T029 → T032 [US3] implementation
```

---

## Implementation Strategy

### MVP First (User Story 1 only)

1. Complete Phase 1 + Phase 2
2. Implement US1 (Phase 3)
3. Validate: create PR with product selection + UI shows product name

### Incremental Delivery

- Add US2 next to unblock manual encoding.
- Add US3 last to harden consistency and improve UX safety.