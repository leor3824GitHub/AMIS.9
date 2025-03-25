using AMIS.Framework.Core.Paging;
using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.Issuances.Get.v1;
using AMIS.WebApi.Catalog.Application.Issuances.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1;

public static class SearchIssuancesEndpoint
{
    internal static RouteHandlerBuilder MapGetIssuanceListEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchIssuancesCommand command) =>
            {
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchIssuancesEndpoint))
            .WithSummary("Gets a list of issuances")
            .WithDescription("Gets a list of issuances with pagination and filtering support")
            .Produces<PagedList<IssuanceResponse>>()
            .RequirePermission("Permissions.Issuances.View")
            .MapToApiVersion(1);
    }
}

