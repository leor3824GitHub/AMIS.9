using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.Suppliers.Delete.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.Supplier;
public static class DeleteSupplierEndpoint
{
    internal static RouteHandlerBuilder MapSupplierDeleteEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapDelete("/{id:guid}", async (Guid id, ISender mediator) =>
             {
                 await mediator.Send(new DeleteSupplierCommand(id));
                 return Results.NoContent();
             })
            .WithName(nameof(DeleteSupplierEndpoint))
            .WithSummary("deletes supplier by id")
            .WithDescription("deletes supplier by id")
            .Produces(StatusCodes.Status204NoContent)
            .RequirePermission("Permissions.Suppliers.Delete")
            .MapToApiVersion(1);
    }
}

