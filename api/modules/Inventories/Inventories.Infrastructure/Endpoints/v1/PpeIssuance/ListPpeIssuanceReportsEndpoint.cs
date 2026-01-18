using AMIS.Framework.Infrastructure.Auth.Policy;
using Carter;
using AMIS.WebApi.Inventories.Application.PpeIssuance.List.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.PpeIssuance;

public static class ListPpeIssuanceReportsEndpoint
{
    public static RouteHandlerBuilder MapListPpeIssuanceReportsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (ISender mediator, CancellationToken cancellationToken) =>
        {
            var response = await mediator.Send(new ListPpeIssuanceReportsQuery(), cancellationToken);
            return Results.Ok(response);
        })
        .WithName(nameof(ListPpeIssuanceReportsEndpoint))
        .WithSummary("List PPE Issuance Reports")
        .WithDescription("Retrieves all PPE Issuance Reports.")
        .Produces<ListPpeIssuanceReportsResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .RequirePermission("Permissions.PpeIssuance.View")
        .MapToApiVersion(1);
    }
}
