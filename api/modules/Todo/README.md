# Todo Module — Domain Summary

Purpose
- Simple example domain used in the repository to illustrate domain events, metrics and DDD patterns.

Key Entity
- `TodoItem` — aggregate root representing a TODO entry with `Title` and `Note`.

Domain Events
- `TodoItemCreated` — queued on creation; handler updates cache and emits a metric.
- `TodoItemUpdated` — queued on updates; handler updates cache and emits a metric.

Metrics
- `TodoMetrics` exposes counters: `Created`, `Updated`, `Deleted` (via `System.Diagnostics.Metrics`).

API / Usage Patterns
- Factory method: `TodoItem.Create(title, note)` to create a new item and queue `TodoItemCreated`.
- Update method: `todo.Update(title, note)` — applies changes and queues `TodoItemUpdated` when necessary.

Auditing
- `TodoItem` derives from `AuditableEntity` (has `Created`, `CreatedBy`, `LastModified`, etc.).

Tests & Examples
- Add unit tests covering:
  - `Create` queues `TodoItemCreated` and increases `Created` metric.
  - `Update` only queues `TodoItemUpdated` when changes are present.

Location
- Domain sources: `api/modules/Todo/` (see `TodoItem.cs`, `Events/`)

If you want, I can add a small unit test project with examples for `TodoItem` creation/update and event handler verification.