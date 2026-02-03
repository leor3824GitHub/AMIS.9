using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.NfaOfficeCodes.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.NfaOfficeCode;

public static class GetNfaOfficeCodeEndpoint
{
    internal static RouteHandlerBuilder MapGetNfaOfficeCodeEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", async (Guid id, ISender mediator) =>
            {
                var response = await mediator.Send(new GetNfaOfficeCodeRequest(id));
                return Results.Ok(response);
            })
            .WithName(nameof(GetNfaOfficeCodeEndpoint))
            .WithSummary("Gets NFA office code by id")
            .WithDescription("Gets NFA office code by id")
            .Produces<NfaOfficeCodeResponse>()
            .RequirePermission("Permissions.NfaOfficeCodes.View")
            .MapToApiVersion(1);
    }
}
