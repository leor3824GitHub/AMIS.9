using AMIS.Framework.Core.Paging;
using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.PpeCategoryCodes.Get.v1;
using AMIS.WebApi.Inventories.Application.PpeCategoryCodes.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.PpeCategoryCode;

public static class SearchPpeCategoryCodesEndpoint
{
    internal static RouteHandlerBuilder MapSearchPpeCategoryCodesEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchPpeCategoryCodesCommand command) =>
            {
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchPpeCategoryCodesEndpoint))
            .WithSummary("Search PPE category codes")
            .WithDescription("Search PPE category codes with pagination and filtering support")
            .Produces<PagedList<PpeCategoryCodeResponse>>()
            .RequirePermission("Permissions.PpeCategoryCodes.View")
            .MapToApiVersion(1);
    }
}
