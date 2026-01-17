using AMIS.Framework.Core.Paging;
using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.JournalEntryVouchers.Search.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.JournalEntryVoucher;

public static class SearchJournalEntryVouchersEndpoint
{
    internal static RouteHandlerBuilder MapSearchJournalEntryVouchersEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/search", async (ISender mediator, [FromBody] SearchJournalEntryVouchersCommand command) =>
            {
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(SearchJournalEntryVouchersEndpoint))
            .Produces<PagedList<JournalEntryVoucherDto>>()
            .RequirePermission("Permissions.JournalEntryVouchers.View")
            .MapToApiVersion(1);
    }

    private static async Task<IResult> SearchHandler(
        ISender mediator,
        [FromBody] SearchJournalEntryVouchersCommand command,
        CancellationToken cancellationToken = default)
    {
        var response = await mediator.Send(command, cancellationToken);
        return Results.Ok(response);
    }
}

