<!--
SYNC IMPACT REPORT

- Version change: template placeholders -> 1.0.0 (initial constitution)
- Modified principles: N/A (filled from template)
- Added sections: Compliance & Architecture Constraints; Development Workflow & Quality Gates
- Removed sections: N/A
- Templates requiring updates:
	- .specify/templates/plan-template.md (updated)
	- .specify/templates/spec-template.md (updated)
	- .specify/templates/tasks-template.md (updated)
	- .specify/templates/commands/*.md: N/A (folder not present)
- Deferred items: None
-->

# AMIS.9 (Asset Management Information System) Constitution

## Core Principles

### Regulatory Compliance & Auditability (NON-NEGOTIABLE)
AMIS MUST remain COA/DBM/PPSAS-aligned and audit-ready.

Non-negotiable rules:
- All inventory and asset workflows MUST preserve an audit trail (who/when/what changed).
- COA/DBM documentary requirements (e.g., RIS/RSMI/ICS/PAR/WMR, inspection and acceptance controls)
	MUST be treated as product requirements, not optional UX.
- RCA mapping (2019) MUST be enforced where required; the system MUST prevent use of outdated mappings
	when policy requires blocking them.
- Any change affecting accounting classification, valuation method, or prescribed forms MUST include a
	compliance impact note in the PR description and be reviewed as a compliance-sensitive change.

Rationale: This system exists to produce defensible, statutory records and reports.

### Clean/Modular Architecture & Boundary Discipline
AMIS MUST preserve Clean Architecture layering and strict module boundaries.

Non-negotiable rules:
- Domain layer MUST be persistence-agnostic and UI-agnostic.
- Application layer MUST define use cases (CQRS) and contracts; Infrastructure provides implementations.
- Cross-module coupling MUST be explicit via shared contracts; avoid leaking EF entities across modules.
- Prefer vertical slice organization for features (Command/Handler/Validator/Endpoint/Response) to keep
	changes localized and reviewable.

Rationale: Modularity enables safe evolution, multi-team contribution, and prevents tight coupling.

### Multi-Tenancy & Authorization by Default
AMIS MUST treat tenant isolation and permissions as first-class invariants.

Non-negotiable rules:
- Every request that touches tenant-scoped data MUST resolve tenant context and enforce isolation.
- Authorization MUST be permission-based for protected operations (create/update/approve/post).
- All tenant-affecting configuration MUST be explicit, validated, and testable.
- Never introduce global/shared data writes without an intentional, documented policy decision.

Rationale: Multi-tenancy is a security and data integrity boundary, not a feature toggle.

### Production-Grade Cloud Readiness
AMIS MUST be operable in production and cloud environments (API + Blazor client).

Non-negotiable rules:
- Configuration MUST be environment-driven (no secrets in code; no environment-specific assumptions).
- Logging MUST be structured and meaningful for incident response (include correlation identifiers when
	available, avoid sensitive data in logs).
- Deployments MUST be reproducible (container-friendly) and support local orchestration (Aspire).
- Backward compatibility MUST be considered for public APIs and generated clients; breaking changes
	require explicit versioning and migration guidance.

Rationale: “Works on my machine” is not acceptable for a compliance system.

### Quality Gates & Safe Change Management
AMIS MUST prefer correctness and traceability over speed.

Non-negotiable rules:
- Changes to business rules, workflows, or state transitions MUST be covered by tests at the
	appropriate level (unit/integration) unless explicitly justified.
- Database migrations MUST be reviewed for safety (idempotence where applicable, data backfill plan
	when needed) and never silently change meaning of historical data.
- Validators MUST be present for externally-facing inputs.
- Avoid unnecessary complexity; choose the simplest design that satisfies compliance and invariants.

Rationale: Quality gates reduce regressions and preserve audit defensibility.

## Compliance & Architecture Constraints

- **Product scope**: PH NFA COA-compliant inventory system covering consumables, semi-expendables, and
	PPE, aligned to COA circulars, DBM issuances, PPSAS/PGAS, and RCA 2019 requirements.
- **Architecture**: Clean Architecture with a framework layer plus self-contained modules.
- **Client/server**: Web API (Minimal APIs with Carter/MediatR/FluentValidation) + Blazor WebAssembly
	client consuming a generated API client (NSwag).
- **Multi-tenancy**: Finbuckle.MultiTenant-based isolation; permissions-based authorization.
- **Data**: EF Core with PostgreSQL primary; MSSQL may exist for migrations/testing scenarios.

## Development Workflow & Quality Gates

- **Feature work** MUST follow vertical slices (Command → Handler → Validator → Endpoint → Response)
	inside the relevant module.
- **API changes** that affect the Blazor client MUST include a regeneration step for the NSwag client.
- **Compliance-sensitive changes** (forms, RCA mapping, thresholds, valuation policy) MUST include:
	(1) a short impact note, (2) test updates, and (3) review by a domain/compliance-aware reviewer.
- **Release readiness** requires: build green, tests green (as applicable), migrations reviewed, and
	no unresolved TODOs related to compliance or data integrity.

## Governance

This constitution is the highest-level engineering governance for AMIS.

Amendment rules:
- Amendments MUST be made via PR, with an explicit rationale and a summary of behavioral impact.
- Versioning MUST follow semantic versioning (MAJOR.MINOR.PATCH):
	- MAJOR: backward-incompatible governance changes, principle removals, or redefinitions
	- MINOR: new principle/section or materially expanded guidance
	- PATCH: clarifications, wording, typo fixes, non-semantic refinements
- PR reviewers MUST verify the “Constitution Check” gates in feature plans are consistent with this
	document.
- When constitution changes, dependent templates under `.specify/templates/` MUST be reviewed and
	updated to match.

**Version**: 1.0.0 | **Ratified**: 2025-12-24 | **Last Amended**: 2025-12-24
