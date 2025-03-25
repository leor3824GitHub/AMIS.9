using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Suppliers.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class UpdateSupplierEndpoint
{
    internal static RouteHandlerBuilder MapSupplierUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (Guid id, UpdateSupplierCommand request, ISender mediator) =>
            {
                if (id != request.Id) return Results.BadRequest();
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateSupplierEndpoint))
            .WithSummary("update a supplier")
            .WithDescription("update a supplier")
            .Produces<UpdateSupplierResponse>()
            .RequirePermission("Permissions.Suppliers.Update")
            .MapToApiVersion(1);
    }
}
