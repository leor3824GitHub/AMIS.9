using Asp.Versioning;
using Carter;
using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.PropertyAcknowledgementReceipt.Cancel.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.PropertyAcknowledgementReceipt;

public static class CancelPAREndpoint
{
    public static RouteHandlerBuilder MapCancelPAREndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/cancel", async (Guid id, ISender mediator) =>
            {
                var command = new CancelPARCommand(id);
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(CancelPAREndpoint))
            .WithSummary("Cancel a Property Acknowledgement Receipt")
            .WithDescription("Cancels a posted PAR, unassigning assets from the custodian. Only Accounting personnel can cancel PARs.")
            .Produces<CancelPARResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission(FshPermission.NameFor(FshActions.Cancel, FshResources.PropertyAcknowledgementReceipt))
            .MapToApiVersion(new ApiVersion(1, 0));
    }
}
