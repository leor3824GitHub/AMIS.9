using AMIS.Framework.Infrastructure.Auth.Policy;
using Carter;
using AMIS.WebApi.Inventories.Application.PropertyAcknowledgementReceipt.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.PropertyAcknowledgementReceipt;

public static class GetPAREndpoint
{
    public static RouteHandlerBuilder MapGetPAREndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (Guid id, ISender mediator, CancellationToken cancellationToken) =>
        {
            var response = await mediator.Send(new GetPARByIdQuery(id), cancellationToken);
            return Results.Ok(response);
        })
        .WithName(nameof(GetPAREndpoint))
        .WithSummary("Get Property Accountability Receipt")
        .WithDescription("Retrieves a specific Property Accountability Receipt by ID.")
        .Produces<GetPARByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .RequirePermission("Permissions.PropertyAcknowledgementReceipt.View")
        .MapToApiVersion(1);
    }
}
