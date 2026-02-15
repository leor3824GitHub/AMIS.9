using AMIS.Framework.Infrastructure.Auth.Policy;
using Carter;
using AMIS.WebApi.Inventories.Application.InventoryCustodianSlips.Features.Post.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.InventoryCustodianSlip;

public static class PostICSEndpoint
{
    public static RouteHandlerBuilder MapPostICSEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id}/post", async (Guid id, ISender mediator) =>
        {
            var command = new PostICSCommand(id);
            var response = await mediator.Send(command);
            return Results.Ok(response);
        })
        .WithName(nameof(PostICSEndpoint))
        .WithSummary("Post Inventory Custodian Slip")
        .WithDescription("Posts (finalizes) an Inventory Custodian Slip, changing status from Draft to Posted.")
        .Produces<PostICSResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .RequirePermission("Permissions.InventoryCustodianSlip.Post")
        .MapToApiVersion(1);
    }
}
