using AMIS.Framework.Infrastructure.Auth.Policy;
using Carter;
using AMIS.WebApi.Inventories.Application.PropertyAcknowledgementReceipt.List.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.PropertyAcknowledgementReceipt;

public static class ListPARsEndpoint
{
    public static RouteHandlerBuilder MapListPARsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (ISender mediator, CancellationToken cancellationToken) =>
        {
            var response = await mediator.Send(new ListPARsQuery(), cancellationToken);
            return Results.Ok(response);
        })
        .WithName(nameof(ListPARsEndpoint))
        .WithSummary("List Property Accountability Receipts")
        .WithDescription("Retrieves all Property Accountability Receipts (PARs).")
        .Produces<ListPARsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .RequirePermission("Permissions.PropertyAcknowledgementReceipt.View")
        .MapToApiVersion(1);
    }
}
