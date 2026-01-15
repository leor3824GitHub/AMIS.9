using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.JournalEntryVouchers.Post.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1.JournalEntryVoucher;

public static class PostJournalEntryVoucherEndpoint
{
    internal static RouteHandlerBuilder MapJournalEntryVoucherPostingEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}/post", async (Guid id, PostJournalEntryVoucherCommand command, ISender mediator) =>
            {
                var response = await mediator.Send(command with { Id = id });
                return Results.Ok(response);
            })
            .WithName(nameof(PostJournalEntryVoucherEndpoint))
            .WithSummary("Posts a journal entry voucher")
            .WithDescription("Posts a journal entry voucher to finalize monthly depreciation entries")
            .Produces<PostJournalEntryVoucherResponse>()
            .RequirePermission("Permissions.JournalEntryVouchers.Post")
            .MapToApiVersion(1);
    }
}
