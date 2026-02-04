using Asp.Versioning;
using Carter;
using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.PropertyAcknowledgementReceipt.Return.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.PropertyAcknowledgementReceipt;

public static class ReturnPAREndpoint
{
    public static RouteHandlerBuilder MapReturnPAREndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/return", async (Guid id, ReturnPARCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                {
                    return Results.BadRequest("Route ID does not match request ID.");
                }

                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(ReturnPAREndpoint))
            .WithSummary("Return a Property Acknowledgement Receipt")
            .WithDescription("Processes the return of assets from a custodian, unassigning the assets.")
            .Produces<ReturnPARResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission($"{FshResources.PropertyAcknowledgementReceipt}.{FshActions.Update}")
            .MapToApiVersion(new ApiVersion(1, 0));
    }
}
