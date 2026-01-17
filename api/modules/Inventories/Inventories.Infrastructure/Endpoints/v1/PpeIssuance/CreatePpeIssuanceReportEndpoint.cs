using AMIS.Framework.Infrastructure.Auth.Policy;
using Carter;
using AMIS.WebApi.Inventories.Application.PpeIssuance.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.PpeIssuance;

public static class CreatePpeIssuanceReportEndpoint
{
    public static RouteHandlerBuilder MapCreatePpeIssuanceReportEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (CreatePpeIssuanceReportCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request);
            return Results.CreatedAtRoute(nameof(GetPpeIssuanceReportEndpoint), new { id = response.Id }, response);
        })
        .WithName(nameof(CreatePpeIssuanceReportEndpoint))
        .WithSummary("Create PPE Issuance Report")
        .WithDescription("Creates a new PPE (Property, Plant & Equipment) Issuance Report for tracking PPE issuance, transfer, or disposal.")
        .Produces<CreatePpeIssuanceReportResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .RequirePermission("Permissions.PpeIssuance.Create")
        .MapToApiVersion(1);
    }
}

