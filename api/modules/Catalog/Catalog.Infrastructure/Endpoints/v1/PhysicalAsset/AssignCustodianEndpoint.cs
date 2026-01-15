using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.PhysicalAssets.AssignCustodian.v1;
using Asp.Versioning;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1.PhysicalAsset;

public static class AssignCustodianEndpoint
{
    public static void MapAssignCustodianEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapPost("/{id:guid}/assign-custodian", async (Guid id, AssignCustodianRequest request, ISender mediator) =>
        {
            var command = new AssignCustodianCommand(id, request.CustodianId);
            var response = await mediator.Send(command);
            return Results.Ok(response);
        })
        .WithName(nameof(AssignCustodianEndpoint))
        .WithSummary("Assign custodian to physical asset")
        .WithDescription("Assigns a custodian/employee to be responsible for a physical asset")
        .Produces<AssignCustodianResponse>()
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .RequirePermission("Permissions.PhysicalAssets.Edit")
        .MapToApiVersion(new ApiVersion(1, 0));
    }

    private sealed record AssignCustodianRequest(Guid CustodianId);
}
