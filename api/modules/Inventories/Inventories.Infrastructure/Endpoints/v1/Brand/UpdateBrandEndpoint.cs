using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.Brands.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.Brand;
public static class UpdateBrandEndpoint
{
    internal static RouteHandlerBuilder MapBrandUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (Guid id, UpdateBrandCommand request, ISender mediator) =>
            {
                if (id != request.Id) return Results.BadRequest();
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdateBrandEndpoint))
            .WithSummary("update a brand")
            .WithDescription("update a brand")
            .Produces<UpdateBrandResponse>()
            .RequirePermission("Permissions.Brands.Update")
            .MapToApiVersion(1);
    }
}

