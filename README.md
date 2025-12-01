# Asset Management Information System (AMIS)

## PH NFA COA‑Compliant Inventory System (Consumables & Office Supplies)

A modular Clean Architecture .NET 9 application designed to support the National Food Authority (NFA) in managing consumables and office supplies in full compliance with Philippine government regulations.

### Regulatory Compliance

This system is designed to comply with:
- **Commission on Audit (COA) Circulars** (e.g., COA Circular 2012‑001, 2022‑002, 2022‑004, and all relevant audit guidelines)
- **Department of Budget and Management (DBM) Issuances** (e.g., DBM Budget Circulars on Inventory and Supplies)
- **Government Procurement Policy Board (GPPB) Guidelines**
- **NFA Internal SOPs and Manuals** (e.g., Property/Asset Management, Procurement, and Warehouse Operations)
- **Philippine Government Accounting Standards (PGAS)**
- **Philippine Public Sector Accounting Standards (PPSAS)**, particularly PPSAS 12 (Inventories)

### Technical Overview

- Backend API: Minimal APIs with Carter, MediatR, FluentValidation, EF Core, Serilog, Hangfire
- Architecture: Clean Architecture with Framework layer + self-contained business Modules
- Client: Blazor WebAssembly with MudBlazor, generated API client via NSwag
- Data: EF Core with PostgreSQL (primary) and MSSQL (for migrations/testing scenarios)
- Orchestration: .NET Aspire for local multi-service development

## System Objectives

### Core Goals
- Ensure **accurate and real‑time tracking** of NFA consumables and office supplies.
- Support **COA‑compliant recording and reporting**, including: 
  - Supplies Ledger Cards (SLC)
  - Stock Cards
  - Report of Supplies and Materials Issued (RSMI)
  - Waste Materials Report
  - Inventory Custodian Slip (ICS) and Property Acknowledgement Receipt (PAR) (for semi‑expendables)
- Maintain **audit trails** for accountability and monitoring.
- Automate **document generation** consistent with required forms.
- Align with **NFA SOPs on Inventory Management**.

### Legal and Regulatory Compliance

#### COA Circulars and Guidelines Integrated
- **COA Circular 2012‑001** – Accounting Guidelines on the Use of Inventory Accounts.
- **COA Circular 2022‑001** – Conversion and proper use of Revised Chart of Accounts (RCA) 2019.
- **COA Circular 2022‑004** – Guidelines for Semi‑Expendable Property (≤ ₱50,000).
- **COA Circular 2005‑002** – Guidelines on Documentary Requirements.
- **COA Audit Templates** – Using SLC, ICS, PAR, RSMI, Waste Material Report.

#### DBM Compliance
- **DBM Budget Circulars** on inventory management and reporting.
- **Annual PFO (Physical Financial Reports)** requirements.
- **PSAS and PPSAS** regarding recognition and measurement of supplies.

#### NFA SOP Alignment
- SOP on **Procurement of Supplies and Materials**.
- SOP on **Warehouse Management and Issuances**.
- SOP on **Property and Supply Management**.

The system enforces **FIFO**, unit cost monitoring, and mandatory documentary requirements for every transaction.

### Core Features

#### Inventory Tracking
- Tracking of consumables and office supplies by: 
  - Stock Number
  - Unit of Measure
  - Beginning Balance
  - Receipts
  - Issuances
  - Ending Balance
- Automatic ledger generation for SLC and Stock Cards.

#### COA‑Compliant Records and Reports
The system automatically produces:
- **SLC (Supplies Ledger Card)**
- **Stock Card** (Warehouse)
- **RSMI (Report of Supplies and Materials Issued)**
- **ICS/PAR** (if semi‑expendable)
- **Waste Materials Report**
- **Inventory Reports for Year‑End COA Audit**
- **Monthly Inventory Summary for Accounting Unit**

#### Accounting Integration
Supports required recognition under **PPSAS 12**:
- Inventory is recorded at **cost**.
- Issuance uses **Weighted Average Method** (for consistency with COA).
- Regular posting to:
  - **Inventory – Supplies**
  - **Inventory – Semi‑Expendables**
  - **Inventory – Office Supplies Issued** (expense)

#### Audit Trail and Accountability
- Logs: Encoded by, Approved by, Released by, Received by.
- All edits require user role authorization.
- Auto‑generated tracking reference numbers per NFA SOP.

### Document Flow (NFA‑Compliant)
1. **Requisitioner files RIS (Requisition and Issue Slip).**
2. **Supply Officer checks stock availability.**
3. **ICS/PAR is generated** for semi‑expendable supplies.
4. **Issuance recorded** in Stock Card and SLC.
5. **RSMI generated** and submitted to Accounting.
6. **Monthly abstract** forwarded to COA Auditor.

## Repository layout

```
AMIS.9.sln
api/
  framework/              # Core + Infrastructure (cross-cutting)
  modules/                # Business modules (Catalog, Todo, ...)
  migrations/             # EF Core migrations projects (PostgreSQL, MSSQL)
  server/                 # API host (startup, DI wiring, endpoints)
apps/
  blazor/                 # Blazor WebAssembly app, infrastructure, shared
aspire/
  Host/                   # Aspire orchestration host
service-defaults/         # Shared service defaults for Aspire
Shared/                   # Shared authorization/resources
```

## Architecture highlights

- Framework layer (`api/framework`)
  - Core: domain primitives, exceptions, specs, paging, storage abstractions
  - Infrastructure: logging, caching, auth, persistence, OpenAPI, security headers, etc.
- Modules (`api/modules/[Module]`)
  - Domain: entities, value objects, domain events
  - Application: commands/queries, DTOs, validators, handlers (CQRS with MediatR)
  - Infrastructure: persistence configs and external integrations
  - Module file: registration and Carter endpoints
- Host (`api/server`)
  - Bootstraps services, registers modules, configures versioning, permissions, OpenAPI
- Client (`apps/blazor`)
  - MudBlazor UI patterns, generated `IApiClient`, JWT auth handler, shared components

### Feature slices

Each feature is organized as vertical slices under `Features/[Operation]/vN` with:
- Command/Query (IRequest<T>)
- Handler
- Validator
- Endpoint (Carter)
- Response DTO

## Prerequisites

- .NET 9 SDK
- Node.js (for some web tooling, if needed)
- PostgreSQL (primary local database)
- Optional: MSSQL (if running alternative migrations)

## Configuration

- API settings: `api/server/appsettings.Development.json`
- Connection strings (PostgreSQL by default). Ensure your Postgres instance is reachable and credentials are correct.
- Centralized package versions in `Directory.Packages.props`

## Quick start

- Build solution: dotnet build AMIS.9.sln
- Run API: dotnet run --project api/server
- Run Blazor client: dotnet run --project apps/blazor/client
- Aspire (optional): dotnet run --project aspire/Host

If you use Aspire, it can wire up dependent services and provide dashboards locally.

## Database and migrations

EF Core migrations are maintained in separate projects under `api/migrations`.

- PostgreSQL migrations (primary): `api/migrations/PostgreSQL`
  - Catalog schema folder: `api/migrations/PostgreSQL/Catalog`
- MSSQL migrations (optional): `api/migrations/MSSQL`

Typical commands for CatalogDbContext with PostgreSQL (run from repo root):

- Create migration (output to Catalog folder):
  - dotnet ef migrations add <Name> --project api/migrations/PostgreSQL --startup-project api/server --context CatalogDbContext -o Catalog
- Apply latest migration:
  - dotnet ef database update --project api/migrations/PostgreSQL --startup-project api/server --context CatalogDbContext

Note: Replace `<Name>` with a descriptive label (e.g., `AddInspectionAndAcceptanceStatuses`).

## Generated API client (NSwag)

- The Blazor app consumes a generated `IApiClient` from the API OpenAPI spec.
- Regenerate client after API changes using the NSwag setup in the Blazor infrastructure project (see `AMIS.nswag` and project targets).
- After regeneration, rebuild the Blazor client to pick up changes.

## Authorization and permissions

- Permission-based: `Shared/Authorization/FshPermissions.cs`
- Use `.RequirePermission("Permissions.Resource.Action")` on endpoints
- Multi-tenant support via Finbuckle.MultiTenant

## UI patterns (Blazor)

- `EntityTable` for CRUD lists with paging
- `PageHeader` for consistent layout
- Fluent validation in forms
- Theme: MudBlazor (light/dark)

## Development workflow

1. Add/modify API endpoints in a module (Domain → Application → Endpoint)
2. Create and apply EF migrations (PostgreSQL)
3. Regenerate the API client (NSwag)
4. Build and run the Blazor client
5. Add tests where appropriate (XUnit)

## Testing

- Test project: `TestProject.XUnit/`
- Run tests via `dotnet test` at the solution level

## COA Circular No. 2022-002 Compliance Integration

This system is enhanced to comply with **COA Circular No. 2022-002 (January 24, 2022)** regarding the **conversion and proper use of Revised Chart of Accounts (RCA) 2019**. Key compliance points applied:

### Alignment with RCA (Updated 2019)
- All consumables, semi-expendable property, office supplies, and MRO items use **updated 2019 RCA account codes**.
- Inventory, issuance, disposal, and reporting modules are mapped to the **Matrix on the Conversion of Accounts (Annex A)**.
- System enforces proper classification under:
  - *Inventories* (Supplies and Materials Inventory, Semi-Expendable (≤ ₱50,000) Inventory)
  - *Expenses* (Supplies and Materials Expense)
  - *Property, Plant and Equipment* (if item exceeds capitalization threshold)

### Journal Entry Voucher (JEV) Compliance
- System auto-generates **COA-compliant JEV templates** following Annex B:
  - Conversion entries
  - Transfer balances
  - Realignment of accounts
- Every transaction (receipt, issuance, consumption, disposal) produces a **linked JEV entry** referencing:
  - Document number (ICS, PAR, RSMI, Waste Material Report, etc.)
  - Accountable officer
  - Updated RCA account code

### Required Documentation & Submission
- All JEVs have options to export:
  - PDF copy for COA Auditor
  - XML/JSON export for GAS-COA
- System maintains a **conversion log** with:
  - Date of mapping
  - RCA version
  - Account transitions
  - Notes to FS requirements

### Revised/Modified Accounts Incorporated
- Trust Liabilities – Disallowances/Charges (20401080) reflected in liability workflows.
- Scholarship Grants/Expenses (50202020) properly categorized for educational/training-related issuances.
- Development in Progress – Copyrights (10898040) available for agencies with intellectual property creation.
- Updated long-form account titles for inter-agency payables.

### Enforcement of Proper Account Use
To prevent misuse of accounts as emphasized by COA:
- System blocks posting under outdated 2015 RCA accounts.
- Validation rules prohibit incorrect classification of:
  - Capitalizable vs. semi-expendable vs. consumable
  - Trust liabilities vs. regular liabilities
  - Inventory vs. expense recognition

### Notes to Financial Statements Automation
The system includes:
- Auto-generated disclosure: *"The Agency completed the conversion from the 2015 RCA to the 2019 RCA as prescribed in COA Circular 2022-002 on [date]."*
- Reference attachment to the conversion JEV.

This ensures full readiness for COA post-audit and annual financial statement preparation.

## Updated Property Classification Rules (COA/DBM 2023–2025 Standards)

To fully comply with the latest COA and DBM issuances, including the updated **₱50,000 threshold for Semi‑Expendable Property**, the system implements the following classification framework:

### Classification Decision Tree (Automated in System)

The system applies the following logic whenever an item is encoded, purchased, received, or issued:

**Step 1 — Determine Acquisition Cost**
- **≤ ₱50,000** → Go to Step 2
- **> ₱50,000** → Automatically classified as **Property, Plant and Equipment (PPE)**

**Step 2 — Nature of Item**
- **Consumable/short‑life items** (used within 1 year) → Classified as **Consumables / Office Supplies**
- **Non‑consumable item with useful life > 1 year** → Classified as **Semi‑Expendable Property**

**Step 3 — Issue Document Requirement**
- **Consumables** → RSMI (Requisition and Issue Slip)
- **Semi‑Expendables (≤ ₱50,000)** → ICS (Inventory Custodian Slip)
- **PPE (> ₱50,000)** → PAR (Property Acknowledgment Receipt)

### Updated Classification Table

| Cost | Type of Item | Classification | Required Document | Relevant Account Code (RCA 2019) |
|------|--------------|----------------|-------------------|----------------------------------|
| **≤ ₱50,000** | Consumable, used within a year | Office/Consumable Supplies | RSMI | **Supplies and Materials Inventory (10501000)** then **Supplies and Materials Expense (50203010)** upon issuance |
| **≤ ₱50,000** | Non‑consumable with >1 year useful life | **Semi‑Expendable Property** | **ICS** | **Semi‑Expendable Property Inventory (10599020)** → Semi‑Expendable Property Expense (50299010) upon issuance |
| **> ₱50,000** | Durable, useful life >1 year | PPE | **PAR** | PPE accounts under **1-06-*** depending on type (e.g., Machinery, ICT Equipment, Furniture & Fixtures) |

All classification is **automatic** and enforced through validation rules.

### Updated JEV Posting Templates (Automated)

#### A. Consumables (Office Supplies)
**Upon Receipt:**
```
Dr: Supplies and Materials Inventory (10501000)
Cr: Accounts Payable / Cash
```

**Upon Issuance:**
```
Dr: Supplies and Materials Expense (50203010)
Cr: Supplies and Materials Inventory (10501000)
```

#### B. Semi‑Expendable Property (≤ ₱50,000)
**Upon Receipt:**
```
Dr: Semi‑Expendable Property Inventory (10599020)
Cr: Accounts Payable / Cash
```

**Upon Issuance (ICS):**
```
Dr: Semi‑Expendable Property Expense (50299010)
Cr: Semi‑Expendable Property Inventory (10599020)
```

ICS is generated automatically and tied to the JEV.

#### C. Property, Plant and Equipment (> ₱50,000)
**Upon Receipt and Issuance of PAR:**
```
Dr: PPE Asset Account (1-06-xx-xxx)
Cr: Accounts Payable / Cash
```

Depreciation entries auto-generated per PPSAS.

### Updated Workflow Integration

System implements three compliant flows:

**A. Consumables Flow**
PO → DR/GRN → Supplies Inventory → RSMI → Expense Recognition

**B. Semi‑Expendable Flow (≤ ₱50,000)**
PO → DR/GRN → Semi‑Expendable Inventory → ICS Issuance → Expense Recognition

**C. PPE Flow (> ₱50,000)**
PO → DR/GRN → PPE Recognition → PAR Assignment → Depreciation

Each path has:  
✔ Correct RCA 2019 account mapping  
✔ Correct COA form (RSMI, ICS, PAR)  
✔ Correct audit logs + JEV generation

### Enforcement Controls
To ensure COA audit readiness:

- Items **cannot be misclassified** (e.g., PPE listed as semi-expendable)  
- Threshold is **system-locked at ₱50,000**, editable only by authorized accounting officials  
- PPE cannot be issued using ICS or RSMI  
- Semi‑Expendables cannot be directly expensed upon receipt  
- Automated reminders if:
  - ICS custodian has pending accountability
  - PPE issued without PAR
  - Supplies inventory balance goes negative

### Notes to Financial Statements (FS) Automation
System auto-generates disclosures:

- Confirmation that classification follows latest COA/DBM thresholds  
- PPE capitalization policy referencing ₱50,000 rule  
- Semi‑expendable policy under COA Circulars
- Inventory valuation and expense recognition policies under PPSAS

All updates fully integrate the **₱50,000 Semi‑Expendable Property threshold** and reinforce COA, DBM, PPSAS, and NFA SOP compliance.

## Domain Rules and Coding Standards

### Coding Standards
- Use **C# (.NET)** with Clean Architecture layering.
- Always follow NFA SOP naming conventions.
- Always generate domain events for stock movements.
- Respect required accounting treatments under **PPSAS 12**.

### Domain Rules

#### Receipts
- Increase stock quantity.
- Update Weighted Average Cost.
- Log reference to **PO/PR No.**

#### Issuances
- Use Weighted Average Cost.
- Automatically generate **RSMI** data.

#### Semi‑Expendables
If unit cost is between **₱1,000–₱50,000**:
- Create ICS or PAR.
- Record under **Inventory – Semi‑Expendable**.

#### Waste or Disposal
- Disposal transactions generate **Waste Materials Report**.
- Update SLC and Stock Cards accordingly.

### Required Output Templates
System can generate:
- SLC
- Stock Card
- RSMI
- ICS
- PAR
- Year‑End Inventory Report

All formats follow **COA and NFA templates**.

### Prohibited Actions
- Do not generate journal entries inconsistent with PPSAS.
- Do not assume FIFO; default is **Weighted Average**.
- Do not modify COA‑prescribed forms.

## Notes for Postgres users

- This repository prioritizes PostgreSQL as the primary database. Ensure your connection string in `api/server/appsettings.Development.json` points to your Postgres instance.
- Existing migrations (e.g., `20251025165044_AddInspectionAndAcceptanceStatuses`) are authored for PostgreSQL and can be applied via the commands above.

## Contributing

- Follow the vertical-slice organization for new features
- Prefer small, focused PRs per feature
- Keep migrations per module and database separate and descriptive
- Update this README when adding major capabilities

---

If you get stuck or see inconsistencies, check `.github/copilot-instructions.md` for project-specific guidance and architecture conventions.

## Employee Registration Enforcement Flow

- User logs in → Identity system authenticates
- User attempts to access any API endpoint
- Middleware checks if `Employee.UserId` matches authenticated user's ID
- If not found → Returns 403 with message to complete registration
- User calls `/self-register` endpoint with employee details
- Employee record created with `UserId` linking to Identity
- Subsequent requests pass through middleware successfully
