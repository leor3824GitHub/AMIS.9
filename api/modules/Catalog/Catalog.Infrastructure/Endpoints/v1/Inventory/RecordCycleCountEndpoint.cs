using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Inventories.RecordCycleCount.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class RecordCycleCountEndpoint
{
    internal static RouteHandlerBuilder MapRecordCycleCountEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/cycle-count", async (Guid id, CycleCountRequest request, ISender mediator) =>
            {
                var command = new RecordCycleCountCommand(id, request.CountedQty, request.CountDate);
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(RecordCycleCountEndpoint))
            .WithSummary("Record inventory cycle count")
            .WithDescription("Records a physical inventory count and adjusts stock quantities")
            .Produces<RecordCycleCountResponse>()
            .RequirePermission("Permissions.Inventories.Update")
            .MapToApiVersion(1);
    }
}

public record CycleCountRequest(int CountedQty, DateTime CountDate);
