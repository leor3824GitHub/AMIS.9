using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.IssuanceItems.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;
public static class CreateIssuanceItemEndpoint
{
    internal static RouteHandlerBuilder MapIssuanceItemCreationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateIssuanceItemCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateIssuanceItemEndpoint))
            .WithSummary("creates a issuanceItem")
            .WithDescription("creates a issuanceItem")
            .Produces<CreateIssuanceItemResponse>()
            .RequirePermission("Permissions.IssuanceItems.Create")
            .MapToApiVersion(1);
    }
}
