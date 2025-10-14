flowchart TD
    A["API Request"] --> B["Endpoint (Endpoints/v1/...)"]
    B --> C["MediatR Handler (Application/...)"]
    C --> D["Repository (Infrastructure/Persistence)"]
    D --> E["DbContext (EF Core)"]
    C --> F["Domain Entity (Domain/...)"]
    B --> G["Authorization (Permissions)"]