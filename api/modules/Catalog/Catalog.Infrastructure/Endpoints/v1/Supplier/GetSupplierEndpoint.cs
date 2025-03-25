using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Suppliers.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class GetSupplierEndpoint
{
    internal static RouteHandlerBuilder MapGetSupplierEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetSupplierRequest(id));
                return Results.Ok(response);
            })
            .WithName(nameof(GetSupplierEndpoint))
            .WithSummary("gets supplier by id")
            .WithDescription("gets supplier by id")
            .Produces<SupplierResponse>()
            .RequirePermission("Permissions.Suppliers.View")
            .MapToApiVersion(1);
    }
}
