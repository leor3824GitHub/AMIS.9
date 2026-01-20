using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.PpeReceiving.List.v1;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.PpeReceiving;

public static class ListPpeReceivingReportsEndpoint
{
    public static RouteHandlerBuilder MapListPpeReceivingReportsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (ISender mediator, CancellationToken cancellationToken) =>
        {
            var response = await mediator.Send(new ListPpeReceivingReportsQuery(), cancellationToken);
            return Results.Ok(response);
        })
        .WithName(nameof(ListPpeReceivingReportsEndpoint))
        .WithSummary("List PPE Receiving Reports")
        .WithDescription("Retrieves all PPE Receiving Reports.")
        .Produces<ListPpeReceivingReportsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .RequirePermission("Permissions.PpeReceiving.View")
        .MapToApiVersion(1);
    }
}
