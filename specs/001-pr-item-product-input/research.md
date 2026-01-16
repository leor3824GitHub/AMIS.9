# Research: Purchase Request Item Product Input

## Decision 1: Persist manual product name explicitly

- Decision: Add a nullable `ManualProductName` field to Purchase Request items and keep `ProductId` nullable.
- Rationale:
  - The current API already supports `ProductId` as nullable, but the UI and responses cannot represent a manual product name without conflating fields.
  - Storing manual product name explicitly avoids overloading `Description` (which is already a distinct field and shown separately in the UI).
  - Keeps auditability: a manual name change is a first-class change tracked via existing auditable entity and domain events.
- Alternatives considered:
  - Store manual name in `Description`: rejected because it mixes meaning (product identity vs item description/specs) and would harm reporting/audit clarity.
  - Create a new “DraftProduct” table: rejected as unnecessary complexity for a localized UX need.

## Decision 2: Enforce XOR validation (exactly one product source)

- Decision: Validate that exactly one of `ProductId` or `ManualProductName` is provided for each PR item.
- Rationale:
  - Prevents ambiguous records and aligns with the feature spec (US3).
  - Ensures downstream purchasing and reporting have a single product source of truth.
- Alternatives considered:
  - Allow both and “prefer ProductId”: rejected because it hides errors and creates silent data loss.

## Decision 3: API contract evolution (backward compatible)

- Decision: Add optional `manualProductName` to request/response DTOs for PR items.
- Rationale:
  - Additive fields are backward compatible for existing clients.
  - UI can cleanly display either selected product name or manual name.
- Alternatives considered:
  - New endpoint dedicated to manual items: rejected (unnecessary API surface; violates simplicity).

## Decision 4: UI interaction (simple toggle)

- Decision: In PurchaseRequestItemList, provide a clear mode toggle for the Product column: “Select from list” vs “Manual entry”.
- Rationale:
  - Minimizes confusion and supports quick encoding.
  - Matches existing MudBlazor patterns (select/text input fields).
