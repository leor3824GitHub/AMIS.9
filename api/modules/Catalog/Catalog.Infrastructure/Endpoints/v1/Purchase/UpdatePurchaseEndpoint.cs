using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Purchases.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class UpdatePurchaseEndpoint
{
    internal static RouteHandlerBuilder MapPurchaseUpdateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}", async (Guid id, UpdatePurchaseCommand request, ISender mediator) =>
            {
                if (id != request.Id) return Results.BadRequest();
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdatePurchaseEndpoint))
            .WithSummary("update a Purchase")
            .WithDescription("update a Purchase")
            .Produces<UpdatePurchaseResponse>()
            .RequirePermission("Permissions.Purchases.Update")
            .MapToApiVersion(1);
    }
}
