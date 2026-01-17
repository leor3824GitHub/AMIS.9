using AMIS.Framework.Infrastructure.Auth.Policy;
using Carter;
using AMIS.WebApi.Inventories.Application.PpeIssuance.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.PpeIssuance;

public static class GetPpeIssuanceReportEndpoint
{
    public static RouteHandlerBuilder MapGetPpeIssuanceReportEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (Guid id, ISender mediator, CancellationToken cancellationToken) =>
        {
            var response = await mediator.Send(new GetPpeIssuanceReportByIdQuery(id), cancellationToken);
            return Results.Ok(response);
        })
        .WithName(nameof(GetPpeIssuanceReportEndpoint))
        .WithSummary("Get PPE Issuance Report")
        .WithDescription("Retrieves a specific PPE Issuance Report by ID.")
        .Produces<GetPpeIssuanceReportByIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .RequirePermission("Permissions.PpeIssuance.View")
        .MapToApiVersion(1);
    }
}

