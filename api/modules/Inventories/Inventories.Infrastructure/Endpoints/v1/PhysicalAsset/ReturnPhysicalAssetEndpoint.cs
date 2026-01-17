using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.PhysicalAssets.Return.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.PhysicalAsset;

/// <summary>
/// Endpoint to return a physical asset from an employee
/// </summary>
public static class ReturnPhysicalAssetEndpoint
{
    internal static RouteHandlerBuilder MapPhysicalAssetReturnEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}/return", async (Guid id, [FromBody] ReturnPhysicalAssetCommand command, ISender mediator) =>
            {
                var returnCommand = new ReturnPhysicalAssetCommand
                {
                    Id = id,
                    Reason = command.Reason,
                    Condition = command.Condition,
                    AcceptedBy = command.AcceptedBy,
                    QuantityReturned = command.QuantityReturned
                };
                var response = await mediator.Send(returnCommand);
                return Results.Ok(response);
            })
            .WithName(nameof(ReturnPhysicalAssetEndpoint))
            .WithSummary("Returns a physical asset from an employee")
            .WithDescription("Records the return of a physical asset from an employee")
            .Produces<ReturnPhysicalAssetResponse>()
            .RequirePermission("Permissions.PhysicalAssets.Return")
            .MapToApiVersion(1);
    }
}

