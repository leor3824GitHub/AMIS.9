using Carter;
using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.PpeReceiving.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.PpeReceiving;

public static class GetPpeReceivingReportEndpoint
{
    public static RouteHandlerBuilder MapGetPpeReceivingReportEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (Guid id, ISender mediator, CancellationToken cancellationToken) =>
        {
            var response = await mediator.Send(new GetPpeReceivingReportByIdQuery(id), cancellationToken);
            return Results.Ok(response);
        })
        .WithName(nameof(GetPpeReceivingReportEndpoint))
        .WithSummary("Get PPE Receiving Report")
        .WithDescription("Retrieves a specific PPE Receiving Report by ID.")
        .Produces<GetPpeReceivingReportByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .RequirePermission("Permissions.PpeReceiving.View")
        .MapToApiVersion(1);
    }
}

