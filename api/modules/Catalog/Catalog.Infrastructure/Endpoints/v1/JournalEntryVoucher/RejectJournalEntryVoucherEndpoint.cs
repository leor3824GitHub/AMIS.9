using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.JournalEntryVouchers.Reject.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1.JournalEntryVoucher;

/// <summary>
/// Endpoint to reject a journal entry voucher
/// </summary>
public static class RejectJournalEntryVoucherEndpoint
{
    internal static RouteHandlerBuilder MapJournalEntryVoucherRejectEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}/reject", async (Guid id, [FromBody] RejectJournalEntryVoucherCommand command, ISender mediator) =>
            {
                var rejectCommand = new RejectJournalEntryVoucherCommand { Id = id, Reason = command.Reason };
                var response = await mediator.Send(rejectCommand);
                return Results.Ok(response);
            })
            .WithName(nameof(RejectJournalEntryVoucherEndpoint))
            .WithSummary("Rejects a journal entry voucher")
            .WithDescription("Rejects a pending JEV and returns it to Draft status")
            .Produces<RejectJournalEntryVoucherResponse>()
            .RequirePermission("Permissions.JournalEntryVouchers.Approve")
            .MapToApiVersion(1);
    }
}
