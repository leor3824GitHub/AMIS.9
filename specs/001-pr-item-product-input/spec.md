# Feature Specification: Purchase Request Item Product Input

**Feature Branch**: `001-pr-item-product-input`  
**Created**: 2025-12-24  
**Status**: Draft  
**Input**: User description: "Modify PurchaseRequestItemList: allow manual product entry or select from product library"

## User Scenarios & Testing *(mandatory)*

<!--
  IMPORTANT: User stories should be PRIORITIZED as user journeys ordered by importance.
  Each user story/journey must be INDEPENDENTLY TESTABLE - meaning if you implement just ONE of them,
  you should still have a viable MVP (Minimum Viable Product) that delivers value.
  
  Assign priorities (P1, P2, P3, etc.) to each story, where P1 is the most critical.
  Think of each story as a standalone slice of functionality that can be:
  - Developed independently
  - Tested independently
  - Deployed independently
  - Demonstrated to users independently
-->

### User Story 1 - Add PR item by selecting an existing product (Priority: P1)

As a supply/procurement user creating a Purchase Request, I want to select a product from the product library so that the PR line uses standardized product details.

**Why this priority**: Standardized products reduce encoding errors and improve reporting consistency.

**Independent Test**: Can be tested by creating a PR item using product selection and verifying the item stores the selected product reference and displays the expected name/unit.

**Acceptance Scenarios**:

1. **Given** a user is editing a Purchase Request, **When** they choose “Select from product list” and pick a product, **Then** the PR item uses the selected product’s name and relevant details.
2. **Given** the product list has multiple matches, **When** the user searches/selects, **Then** they can choose the intended product without manually typing the full name.

---

### User Story 2 - Add PR item by manual product entry (Priority: P2)

As a supply/procurement user, I want to type the product name manually so that I can encode PR items even when the product is not yet in the library.

**Why this priority**: Ensures continuity of operations when the product master list is incomplete.

**Independent Test**: Can be tested by creating a PR item using manual entry and verifying it is saved and displayed as entered.

**Acceptance Scenarios**:

1. **Given** a user is editing a Purchase Request, **When** they choose “Manual entry” and input a product name, **Then** the PR item is saved with that manual name.
2. **Given** the product is manually entered, **When** the PR item is viewed later, **Then** it clearly indicates it is a manual entry (not a library product).

---

### User Story 3 - Prevent inconsistent product input (Priority: P3)

As a user, I want the system to prevent ambiguous product details so that each PR item is either clearly linked to a library product or clearly recorded as manual.

**Why this priority**: Prevents downstream confusion in approvals, purchasing, and reporting.

**Independent Test**: Can be tested by switching between “Select” and “Manual” modes and validating that only one source of truth is retained.

**Acceptance Scenarios**:

1. **Given** a PR item is in “Select from product list” mode with a chosen product, **When** the user switches to “Manual entry”, **Then** the selected product reference is cleared and only the manual fields remain.
2. **Given** a PR item is in “Manual entry” mode with a typed name, **When** the user switches to “Select from product list”, **Then** the manual name is cleared and only the selected product reference remains.

---

[Add more user stories as needed, each with an assigned priority]

### Edge Cases

- Manual name is empty/whitespace.
- User attempts to save an item with both a selected product and a manual product name.
- Selected product becomes unavailable (deleted/disabled) after being chosen.
- Product list contains similarly-named items (risk of wrong selection).
- User edits an existing PR item and changes the product input mode.

## Requirements *(mandatory)*

<!--
  ACTION REQUIRED: The content in this section represents placeholders.
  Fill them out with the right functional requirements.
-->

### Functional Requirements

- **FR-001**: The system MUST allow users to choose the PR item’s product input method: (a) select from product library or (b) manual entry.
- **FR-002**: When “select from product library” is used, the PR item MUST be linked to exactly one existing product from the library.
- **FR-003**: When “manual entry” is used, the PR item MUST store a user-provided product name and MUST NOT require a product library link.
- **FR-004**: A PR item MUST NOT be saved with both a product library link and a manual product name at the same time.
- **FR-005**: The system MUST clearly indicate (in the PR item UI and view) whether the product came from the library or was manually entered.
- **FR-006**: The system MUST validate required fields based on the chosen input method (e.g., manual product name required in manual mode).
- **FR-007**: The product library selection MUST only show products accessible within the current tenant context.
- **FR-008**: The change of input method MUST be an auditable event for the PR item (who changed, when).

**AMIS constraints to apply when writing FRs**:
- Compliance-sensitive workflows MUST include audit trail and documentary outputs where applicable.
- Multi-tenant data MUST be isolated by tenant context.
- Clean Architecture boundaries MUST be respected (no UI/persistence leakage into Domain).

### Key Entities *(include if feature involves data)*

- **Purchase Request (PR)**: A request document containing one or more requested items.
- **Purchase Request Item**: A line item on a PR with requested quantity and product details.
- **Product (Library Item)**: A standardized product entry maintained in the product library for selection.
- **Product Input Method**: Indicates whether the PR item uses a library product link or manual entry.

## Success Criteria *(mandatory)*

<!--
  ACTION REQUIRED: Define measurable success criteria.
  These must be technology-agnostic and measurable.
-->

### Measurable Outcomes

- **SC-001**: Users can add a PR item using product selection in under 30 seconds (excluding typing/search time variance).
- **SC-002**: Users can add a PR item via manual entry in under 20 seconds.
- **SC-003**: At least 95% of attempted saves are successful on the first try (validation messages are clear enough to correct quickly).
- **SC-004**: Zero PR items are saved with ambiguous product source (both manual and library populated).
