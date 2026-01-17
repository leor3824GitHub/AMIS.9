using Carter;
using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.PpeReceiving.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.PpeReceiving;

public static class CreatePpeReceivingReportEndpoint
{
    public static RouteHandlerBuilder MapCreatePpeReceivingReportEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (CreatePpeReceivingReportCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request);
            return Results.CreatedAtRoute(nameof(GetPpeReceivingReportEndpoint), new { id = response.Id }, response);
        })
        .WithName(nameof(CreatePpeReceivingReportEndpoint))
        .WithSummary("Create PPE Receiving Report")
        .WithDescription("Creates a new PPE (Property, Plant & Equipment) Receiving Report for tracking PPE receipt from suppliers, transfers, or donations.")
        .Produces<CreatePpeReceivingReportResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .RequirePermission("Permissions.PpeReceiving.Create")
        .MapToApiVersion(1);
    }
}

