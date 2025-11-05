using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Inventories.Quarantine.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class QuarantineInventoryEndpoint
{
    internal static RouteHandlerBuilder MapQuarantineInventoryEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/quarantine", async (Guid id, QuarantineRequest request, ISender mediator) =>
            {
                var command = new QuarantineInventoryCommand(id, request.Location);
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(QuarantineInventoryEndpoint))
            .WithSummary("Quarantine inventory")
            .WithDescription("Marks inventory as quarantined with optional location")
            .Produces<QuarantineInventoryResponse>()
            .RequirePermission("Permissions.Inventories.Update")
            .MapToApiVersion(1);
    }
}

public sealed record QuarantineRequest(string? Location);
