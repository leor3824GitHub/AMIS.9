using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Catalog.Application.JournalEntryVouchers.Approve.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Catalog.Infrastructure.Endpoints.v1.JournalEntryVoucher;

/// <summary>
/// Endpoint to approve a journal entry voucher
/// </summary>
public static class ApproveJournalEntryVoucherEndpoint
{
    internal static RouteHandlerBuilder MapJournalEntryVoucherApproveEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}/approve", async (Guid id, ISender mediator) =>
            {
                var command = new ApproveJournalEntryVoucherCommand { Id = id };
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(ApproveJournalEntryVoucherEndpoint))
            .WithSummary("Approves a journal entry voucher")
            .WithDescription("Approves a pending JEV and posts it to the ledger")
            .Produces<ApproveJournalEntryVoucherResponse>()
            .RequirePermission("Permissions.JournalEntryVouchers.Approve")
            .MapToApiVersion(1);
    }
}
