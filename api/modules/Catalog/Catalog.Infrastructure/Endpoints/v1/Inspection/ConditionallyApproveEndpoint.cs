using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Inspections.ConditionallyApprove.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class ConditionallyApproveEndpoint
{
    internal static RouteHandlerBuilder MapConditionallyApproveEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/{id:guid}/conditionally-approve", async (Guid id, ConditionalApprovalRequest request, ISender mediator) =>
            {
                var command = new ConditionallyApproveCommand(id, request.Conditions);
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(ConditionallyApproveEndpoint))
            .WithSummary("Conditionally approve inspection")
            .WithDescription("Approves inspection with specific conditions that must be met")
            .RequirePermission("Permissions.Inspections.Approve")
            .Produces<ConditionallyApproveResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .MapToApiVersion(1);
    }
}

public record ConditionalApprovalRequest(string Conditions);
