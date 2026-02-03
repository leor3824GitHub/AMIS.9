using AMIS.Framework.Core.Paging;
using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.NfaOfficeCodes.Get.v1;
using AMIS.WebApi.Inventories.Application.NfaOfficeCodes.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.NfaOfficeCode;

public static class SearchNfaOfficeCodesEndpoint
{
    internal static RouteHandlerBuilder MapSearchNfaOfficeCodesEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchNfaOfficeCodesCommand command) =>
            {
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchNfaOfficeCodesEndpoint))
            .WithSummary("Search NFA office codes")
            .WithDescription("Search NFA office codes with pagination and filtering support")
            .Produces<PagedList<NfaOfficeCodeResponse>>()
            .RequirePermission("Permissions.NfaOfficeCodes.View")
            .MapToApiVersion(1);
    }
}
