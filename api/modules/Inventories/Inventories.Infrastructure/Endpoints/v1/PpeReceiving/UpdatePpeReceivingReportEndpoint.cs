using Asp.Versioning;
using Carter;
using AMIS.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.PpeReceiving;

public static class UpdatePpeReceivingReportEndpoint
{
    public static RouteHandlerBuilder MapUpdatePpeReceivingReportEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (Guid id, Application.PpeReceiving.Update.v1.UpdatePpeReceivingReportCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                {
                    return Results.BadRequest("Route ID does not match request ID.");
                }

                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdatePpeReceivingReportEndpoint))
            .WithSummary("Update a PPE Receiving Report (Draft only)")
            .WithDescription("Updates header and line items of a PPE Receiving Report in Draft status.")
            .Produces<Application.PpeReceiving.Update.v1.UpdatePpeReceivingReportResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission($"{FshResources.Pper}.{FshActions.Update}")
            .MapToApiVersion(1);
    }
}
