using AMIS.Framework.Infrastructure.Auth.Policy;
using Carter;
using AMIS.WebApi.Inventories.Application.PropertyAcknowledgementReceipt.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.PropertyAcknowledgementReceipt;

public static class CreatePAREndpoint
{
    public static RouteHandlerBuilder MapCreatePAREndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (CreatePARCommand request, ISender mediator) =>
        {
            var response = await mediator.Send(request);
            return Results.CreatedAtRoute(nameof(GetPAREndpoint), new { id = response.Id }, response);
        })
        .WithName(nameof(CreatePAREndpoint))
        .WithSummary("Create Property Acknowledgement Receipt")
        .WithDescription("Creates a new Property Acknowledgement Receipt (PAR) for assigning PPE to individual custodians/employees.")
        .Produces<CreatePARResponse>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .RequirePermission("Permissions.PropertyAcknowledgementReceipt.Create")
        .MapToApiVersion(1);
    }
}
