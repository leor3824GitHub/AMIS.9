using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.Disposals.Request.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.AssetDisposal;

public static class CreateDisposalRequestEndpoint
{
    internal static RouteHandlerBuilder MapCreateDisposalRequestEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("", async (CreateDisposalRequestCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Created($"/api/disposals/{response.DisposalId}", response);
            })
            .WithName(nameof(CreateDisposalRequestEndpoint))
            .WithSummary("Request asset disposal")
            .WithDescription("Create a new disposal request for an asset. Initiates the disposal workflow.")
            .Produces<CreateDisposalRequestResponse>(StatusCodes.Status201Created)
            .WithOpenApi()
            .RequirePermission("Permissions.Disposals.Request")
            .MapToApiVersion(1);
    }
}
