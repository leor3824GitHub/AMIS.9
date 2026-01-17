using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.Suppliers.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.Supplier;
public static class CreateSupplierEndpoint
{
    internal static RouteHandlerBuilder MapSupplierCreationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateSupplierCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateSupplierEndpoint))
            .WithSummary("creates a supplier")
            .WithDescription("creates a supplier")
            .Produces<CreateSupplierResponse>()
            .RequirePermission("Permissions.Suppliers.Create")
            .MapToApiVersion(1);
    }
}

