using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Purchases.Delete.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1
{
    public static class DeletePurchasesEndpoint
    {
        internal static RouteHandlerBuilder MapPurchasesDeleteEndpoint(this IEndpointRouteBuilder endpoints)
        {
            return endpoints
                .MapDelete("/", async ([FromBody] List<Guid> purchaseIds, ISender mediator) =>
                {
                    if (purchaseIds == null || purchaseIds.Count == 0)
                    {
                        return Results.BadRequest("Purchase IDs cannot be null or empty.");
                    }

                    try
                    {
                        await mediator.Send(new DeletePurchasesCommand(purchaseIds));
                        return Results.NoContent();
                    }
                    catch (Exception)
                    {
                        return Results.StatusCode(StatusCodes.Status500InternalServerError);
                    }
                })
                .WithName(nameof(DeletePurchasesEndpoint))
                .WithSummary("Deletes purchases by IDs")
                .WithDescription("Deletes purchases by IDs")
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequirePermission("Permissions.Purchases.Delete")
                .MapToApiVersion(1);
        }
    }
}
