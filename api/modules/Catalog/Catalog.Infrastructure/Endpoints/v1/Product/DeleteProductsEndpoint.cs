using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Products.Delete.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1
{
    public static class DeleteProductsEndpoint
    {
        internal static RouteHandlerBuilder MapProductsDeleteEndpoint(this IEndpointRouteBuilder endpoints)
        {
            return endpoints
                .MapDelete("/", async ([FromBody] List<Guid> productIds, ISender mediator) =>
                {
                    if (productIds == null || productIds.Count == 0)
                    {
                        return Results.BadRequest("Product IDs cannot be null or empty.");
                    }

                    try
                    {
                        await mediator.Send(new DeleteProductsCommand(productIds));
                        return Results.NoContent();
                    }
                    catch (Exception)
                    {
                        return Results.StatusCode(StatusCodes.Status500InternalServerError);
                    }
                })
                .WithName(nameof(DeleteProductsEndpoint))
                .WithSummary("Deletes products by IDs")
                .WithDescription("Deletes products by IDs")
                .Produces(StatusCodes.Status204NoContent)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces(StatusCodes.Status500InternalServerError)
                .RequirePermission("Permissions.Products.Delete")
                .MapToApiVersion(1);
        }
    }
}
