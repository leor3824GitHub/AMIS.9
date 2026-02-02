# AMIS.9 API Architecture Diagram

## System Architecture Overview

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                        HTTP Clients (Blazor, Postman, etc.)                 │
└──────────────────────────────┬──────────────────────────────────────────────┘
                               │
                        ┌──────▼──────┐
                        │  API Server │
                        │ (net9.0)    │
                        └──────┬──────┘
                               │
        ┌──────────────────────┼──────────────────────┐
        │                      │                      │
        ▼                      ▼                      ▼
   ┌──────────┐         ┌──────────┐          ┌──────────┐
   │ Framework│         │ Inventories         │   Todo   │
   │ Layer    │         │  Module  │          │ Module   │
   └──────────┘         └──────────┘          └──────────┘
```

## Layered Architecture (Per Module)

```
┌─────────────────────────────────────────────────────────┐
│  PRESENTATION LAYER (Web/API)                           │
│  ├─ Carter Endpoints (Minimal APIs)                     │
│  ├─ Route Handlers                                      │
│  └─ Authorization Filters                              │
└──────────────────────┬──────────────────────────────────┘
                       │
┌──────────────────────▼──────────────────────────────────┐
│  APPLICATION LAYER (CQRS)                              │
│  ├─ Commands (IRequest<TResponse>)                     │
│  ├─ Handlers (IRequestHandler<TCommand, TResponse>)   │
│  ├─ Validators (AbstractValidator<TCommand>)          │
│  ├─ Responses (DTOs)                                  │
│  └─ MediatR Pipeline Behaviors                        │
└──────────────────────┬──────────────────────────────────┘
                       │
┌──────────────────────▼──────────────────────────────────┐
│  DOMAIN LAYER (Business Logic)                         │
│  ├─ Entities (Brand, Product, etc.)                   │
│  ├─ Value Objects                                     │
│  ├─ Domain Events                                     │
│  ├─ Aggregate Roots (IAggregateRoot)                 │
│  └─ Factory Methods                                   │
└──────────────────────┬──────────────────────────────────┘
                       │
┌──────────────────────▼──────────────────────────────────┐
│  INFRASTRUCTURE LAYER (Data & External Services)       │
│  ├─ Entity Framework DbContext                        │
│  ├─ Repository Pattern                                │
│  ├─ Database Initializers                             │
│  └─ Persistence Configurations                        │
└──────────────────────┬──────────────────────────────────┘
                       │
┌──────────────────────▼──────────────────────────────────┐
│  DATA LAYER                                             │
│  ├─ PostgreSQL Database                               │
│  ├─ MSSQL Database                                    │
│  └─ Migrations                                        │
└─────────────────────────────────────────────────────────┘
```

## Module Structure Pattern

```
Module (e.g., Inventories)
│
├─ Inventories.Domain/
│  ├─ Entities/
│  │  └─ Brand.cs
│  ├─ Events/
│  │  ├─ BrandCreated.cs
│  │  └─ BrandUpdated.cs
│  └─ IAggregateRoot.cs
│
├─ Inventories.Application/
│  ├─ Brands/
│  │  ├─ Create/v1/
│  │  │  ├─ CreateBrandCommand.cs (IRequest<TResponse>)
│  │  │  ├─ CreateBrandCommandValidator.cs
│  │  │  ├─ CreateBrandHandler.cs (IRequestHandler<,>)
│  │  │  └─ CreateBrandResponse.cs
│  │  ├─ Get/v1/
│  │  ├─ Update/v1/
│  │  ├─ Delete/v1/
│  │  └─ GetList/v1/
│  └─ [Other aggregates...]
│
├─ Inventories.Infrastructure/
│  ├─ Endpoints/v1/
│  │  └─ Brand/
│  │     ├─ CreateBrandEndpoint.cs (MapBrandCreationEndpoint)
│  │     ├─ GetBrandEndpoint.cs
│  │     └─ [Other endpoints...]
│  ├─ Persistence/
│  │  ├─ InventoriesDbContext.cs
│  │  ├─ InventoriesRepository.cs
│  │  └─ InventoriesDbInitializer.cs
│  └─ Middleware/
│
└─ InventoriesModule.cs
   ├─ Endpoints : CarterModule (routing)
   └─ RegisterInventoriesServices() (DI)
```

## CQRS Flow Diagram

```
HTTP Request (POST /api/v1/inventories/brands)
│
├─ Validation Layer
│  ├─ Route Handler captures CreateBrandCommand
│  └─ MediatR ValidationBehavior validates
│
├─ Command Handler
│  ├─ Handler receives validated command
│  ├─ Repository.AddAsync(brand)
│  ├─ Logger records action
│  └─ Returns CreateBrandResponse
│
├─ Domain Event Publishing
│  ├─ DbContext publishes BrandCreated event
│  └─ Event handlers process (email, audit, etc.)
│
└─ HTTP Response (200 OK + Response DTO)
```

## Dependency Injection Container

```
Services Collection
│
├─ Framework Services (api/framework/)
│  ├─ Authentication (JWT)
│  ├─ Authorization (Permission-based)
│  ├─ Caching (Redis/Memory)
│  ├─ Logging (Serilog)
│  ├─ OpenAPI (Swagger/OpenAPI)
│  ├─ Persistence (EF Core)
│  ├─ Multi-Tenancy (Finbuckle)
│  └─ OpenTelemetry (Observability)
│
├─ Module Services (api/modules/)
│  ├─ Inventories
│  │  ├─ DbContext: InventoriesDbContext
│  │  ├─ Repositories (Keyed)
│  │  │  ├─ IRepository<Brand> @ "inventories:brands"
│  │  │  ├─ IRepository<Product> @ "inventories:products"
│  │  │  └─ [30+ entity types]
│  │  └─ Initializer: InventoriesDbInitializer
│  │
│  └─ Todo
│     ├─ DbContext: TodoDbContext
│     ├─ Repository (Keyed)
│     │  └─ IRepository<TodoItem> @ "todo"
│     └─ Initializer: TodoDbInitializer
│
├─ Validators (FluentValidation)
│  └─ AutoRegistered from Assembly
│
├─ MediatR
│  ├─ Commands (IRequest<TResponse>)
│  ├─ Handlers (IRequestHandler<,>)
│  └─ Pipeline Behaviors
│     └─ ValidationBehavior
│
└─ Carter Modules
   ├─ InventoriesModule.Endpoints
   └─ TodoModule.Endpoints
```

## Authorization & Permission Flow

```
HTTP Request with JWT Token
│
├─ Token Extraction
│  └─ JWT Bearer Scheme
│
├─ Principal Extraction
│  ├─ Claims parsing
│  └─ User identification
│
├─ Permission Check (Endpoint)
│  ├─ RequirePermission("Permissions.Brands.Create")
│  ├─ Permission resolved from FshPermissions
│  └─ Checked against user role
│
├─ Decision
│  ├─ ✅ Permitted → Continue to Handler
│  └─ ❌ Denied → 403 Forbidden
│
└─ Handler Execution (if authorized)
```

## Database Context Architecture

```
DbContext Hierarchy
│
├─ FshDbContext (Framework base class)
│  ├─ Multi-tenancy support
│  ├─ Domain event publishing
│  ├─ Audit trail
│  └─ Global query filters
│
└─ Module-Specific DbContexts
   ├─ InventoriesDbContext
   │  ├─ DbSet<Brand>
   │  ├─ DbSet<Product>
   │  ├─ DbSet<PhysicalAsset>
   │  └─ [30+ entity types for procurement & asset mgmt]
   │
   ├─ TodoDbContext
   │  └─ DbSet<TodoItem>
   │
   └─ Migrations (Separate per database)
      ├─ PostgreSQL/Migrations/
      │  ├─ Inventories/
      │  │  └─ 20250123051259_InitialInventories.cs
      │  └─ Todo/
      │
      └─ MSSQL/Migrations/
         ├─ Inventories/
         └─ Todo/
```

## Request Processing Pipeline

```
┌─────────────────────────────────────────────┐
│ 1. HTTP Request arrives at API              │
└────────────┬────────────────────────────────┘
             │
┌────────────▼────────────────────────────────┐
│ 2. CORS Policy (if cross-origin)            │
└────────────┬────────────────────────────────┘
             │
┌────────────▼────────────────────────────────┐
│ 3. Rate Limiting (per-endpoint/per-user)   │
└────────────┬────────────────────────────────┘
             │
┌────────────▼────────────────────────────────┐
│ 4. Security Headers (X-Frame-Options, etc.) │
└────────────┬────────────────────────────────┘
             │
┌────────────▼────────────────────────────────┐
│ 5. Multi-Tenancy Middleware                 │
│    (Extract tenant from request)            │
└────────────┬────────────────────────────────┘
             │
┌────────────▼────────────────────────────────┐
│ 6. Route to Appropriate Endpoint            │
└────────────┬────────────────────────────────┘
             │
┌────────────▼────────────────────────────────┐
│ 7. Authentication (JWT Validation)          │
└────────────┬────────────────────────────────┘
             │
┌────────────▼────────────────────────────────┐
│ 8. Authorization (Permission Check)         │
│    RequirePermission("...")                 │
└────────────┬────────────────────────────────┘
             │
┌────────────▼────────────────────────────────┐
│ 9. Model Binding & Deserialization          │
│    CreateBrandCommand extracted from body   │
└────────────┬────────────────────────────────┘
             │
┌────────────▼────────────────────────────────┐
│ 10. MediatR Send to Handler                 │
│     └─ ValidationBehavior runs              │
│     └─ IRequestHandler<> executes           │
└────────────┬────────────────────────────────┘
             │
┌────────────▼────────────────────────────────┐
│ 11. Repository Persistence                  │
│     └─ EF Core change tracking              │
└────────────┬────────────────────────────────┘
             │
┌────────────▼────────────────────────────────┐
│ 12. Domain Event Publishing                 │
│     └─ Event handlers execute               │
└────────────┬────────────────────────────────┘
             │
┌────────────▼────────────────────────────────┐
│ 13. Response Created                        │
│     └─ DTO returned to client               │
└────────────┬────────────────────────────────┘
             │
┌────────────▼────────────────────────────────┐
│ 14. Structured Logging                      │
│     └─ Serilog records action               │
└────────────┬────────────────────────────────┘
             │
┌────────────▼────────────────────────────────┐
│ 15. HTTP Response (200 OK + body)           │
└─────────────────────────────────────────────┘
```

## Scalability Architecture

```
Load Balancer (Round-robin)
│
├─ API Server Instance 1
│  └─ Stateless handlers + cache
├─ API Server Instance 2
│  └─ Stateless handlers + cache
└─ API Server Instance N
   └─ Stateless handlers + cache

All instances connect to:
├─ Shared PostgreSQL Primary
├─ PostgreSQL Read Replicas (optional)
├─ Redis Cache (distributed)
├─ Message Queue (for events)
└─ Elasticsearch (centralized logging)
```

## Technology Stack Summary

```
Presentation
├─ ASP.NET Core 9.0
├─ Carter (Minimal APIs)
└─ OpenAPI/Swagger

Application
├─ MediatR (CQRS)
├─ FluentValidation
└─ AutoMapper (optional)

Domain
├─ C# Records (immutability)
├─ Domain-Driven Design
└─ Domain Events

Infrastructure
├─ Entity Framework Core 9.0
├─ Finbuckle.MultiTenant
├─ Serilog (Structured Logging)
├─ OpenTelemetry (Tracing/Metrics)
└─ JWT Authentication

Data
├─ PostgreSQL (Primary)
├─ SQL Server (Alternative)
├─ Redis (Caching)
└─ Elasticsearch (Logging)

DevOps
├─ .NET Aspire (Orchestration)
├─ Docker (Containerization)
├─ NSwag (API Client Generation)
└─ xUnit (Testing)
```

---

This architecture is **clean, modular, scalable, and production-ready**.

