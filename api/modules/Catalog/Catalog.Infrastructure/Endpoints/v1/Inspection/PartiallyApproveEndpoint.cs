using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Inspections.PartiallyApprove.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class PartiallyApproveEndpoint
{
    internal static RouteHandlerBuilder MapPartiallyApproveEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/partially-approve", async (Guid id, PartialApprovalRequest request, ISender mediator) =>
            {
                var command = new PartiallyApproveCommand(id, request.PartialDetails);
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(PartiallyApproveEndpoint))
            .WithSummary("Partially approve inspection")
            .WithDescription("Approves inspection with partial acceptance details")
            .RequirePermission("Permissions.Inspections.Approve")
            .Produces<PartiallyApproveResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .MapToApiVersion(1);
    }
}

public record PartialApprovalRequest(string PartialDetails);
