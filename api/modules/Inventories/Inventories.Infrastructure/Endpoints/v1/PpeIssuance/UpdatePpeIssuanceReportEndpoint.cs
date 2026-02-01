using Asp.Versioning;
using Carter;
using AMIS.Framework.Infrastructure.Auth.Policy;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Shared.Authorization;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.PpeIssuance;

public static class UpdatePpeIssuanceReportEndpoint
{
    public static RouteHandlerBuilder MapUpdatePpeIssuanceReportEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (Guid id, Application.PpeIssuance.Update.v1.UpdatePpeIssuanceReportCommand request, ISender mediator) =>
            {
                if (id != request.Id)
                {
                    return Results.BadRequest("Route ID does not match request ID.");
                }

                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(UpdatePpeIssuanceReportEndpoint))
            .WithSummary("Update a PPE Issuance Report (Draft only)")
            .WithDescription("Updates header and line items of a PPE Issuance Report in Draft status.")
            .Produces<Application.PpeIssuance.Update.v1.UpdatePpeIssuanceReportResponse>()
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .RequirePermission($"{FshResources.PpeIssuance}.{FshActions.Update}")
            .MapToApiVersion(1);
    }
}
