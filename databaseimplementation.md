
# Database Implementation in AMIS.9

## Overview
This document focuses on database-specific configuration, access patterns, and multi-tenancy database strategy options in AMIS.9. For architectural and workflow details, see [multitenantimplementation.md](multitenantimplementation.md).

## Multi-Tenancy Database Access Options

AMIS.9 supports two main strategies for multi-tenant database access:

### 1. Single Database for All Tenants
- All tenants share the same physical database.
- Tenant data is separated logically, typically by a TenantId column in each table.
- The connection string is the same for all tenants; Finbuckle.MultiTenant resolves the current tenant context for filtering.
- **Pros:** Easier to manage, fewer databases, simpler migrations.
- **Cons:** Data isolation is logical only; risk of cross-tenant data access if filtering is not enforced.

**Configuration:**
Set all tenants to use the same connection string in their configuration or leave the tenant-specific connection string empty to default to the shared one.

**Example:**
```csharp
// All tenants use the same connection string
tenant.ConnectionString = string.Empty; // or same value for all
```

### 2. One Database per Tenant
- Each tenant has a dedicated database (or schema).
- The connection string is unique per tenant and stored in the tenant info.
- Finbuckle.MultiTenant resolves the correct connection string for each request.
- **Pros:** Strong data isolation, easier per-tenant backup/restore, better for large tenants.
- **Cons:** More complex to manage, more databases, migrations must be applied to each database.

**Configuration:**
Set a unique connection string for each tenant in their configuration or during provisioning.

**Example:**
```csharp
// Each tenant has its own connection string
tenant.ConnectionString = "Server=...;Database=TenantA;...";
```

### Code Impact
- The application code (DbContext, repositories, etc.) remains the same for both strategies.
- The difference is in how the connection string is resolved for each tenant by Finbuckle.MultiTenant.
- Migrations and seeding are performed per database/tenant as needed.

## Database Access Patterns

- Data access is performed via repositories or direct DbContext usage, using async EF Core methods and LINQ.
- Each module defines its own DbContext (e.g., `CatalogDbContext`, `TodoDbContext`).
- For migration and seeding logic, see [multitenantimplementation.md](multitenantimplementation.md#full-code-samples).

## Further Reading

- For architectural and workflow details, including code samples for tenant initialization, DbInitializer, and per-tenant migration/seeding, see [multitenantimplementation.md](multitenantimplementation.md).

---
This document describes the database implementation as of November 2025 for the AMIS.9 project.
