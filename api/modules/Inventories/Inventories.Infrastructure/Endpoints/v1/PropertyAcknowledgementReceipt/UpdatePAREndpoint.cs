using Asp.Versioning;
using Carter;
using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.PropertyAcknowledgementReceipt.Update.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.PropertyAcknowledgementReceipt;

public static class UpdatePAREndpoint
{
    public static RouteHandlerBuilder MapUpdatePAREndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (Guid id, UpdatePARCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                {
                    return Results.BadRequest("Route ID does not match request ID.");
                }

                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdatePAREndpoint))
            .WithSummary("Update a Property Acknowledgement Receipt (Draft only)")
            .WithDescription("Updates header and line items of a PAR in Draft status.")
            .Produces<UpdatePARResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission($"{FshResources.PropertyAcknowledgementReceipt}.{FshActions.Update}")
            .MapToApiVersion(new ApiVersion(1, 0));
    }
}
