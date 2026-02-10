using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.PhysicalAssets.UpdateCondition.v1;
using Asp.Versioning;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.PhysicalAsset;

public static class UpdateConditionEndpoint
{
    public static void MapUpdateConditionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/{id:guid}/update-condition", async (Guid id, UpdateConditionRequest request, ISender mediator) =>
        {
            var command = new UpdateConditionCommand(id, request.Condition, request.Remarks);
            var response = await mediator.Send(command);
            return Results.Ok(response);
        })
        .WithName(nameof(UpdateConditionEndpoint))
        .WithSummary("Update physical asset condition")
        .WithDescription("Updates the condition status of a physical asset (e.g., Good, Fair, Poor)")
        .Produces<UpdateConditionResponse>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission("Permissions.PhysicalAssets.Update")
        .MapToApiVersion(new ApiVersion(1, 0));
    }

    private sealed record UpdateConditionRequest(string Condition, string? Remarks = null);
}

