using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.JournalEntryVouchers.Submit.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.JournalEntryVoucher;

/// <summary>
/// Endpoint to submit a journal entry voucher for approval
/// </summary>
public static class SubmitJournalEntryVoucherEndpoint
{
    internal static RouteHandlerBuilder MapJournalEntryVoucherSubmitEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPut("/{id:guid}/submit", async (Guid id, ISender mediator) =>
            {
                var command = new SubmitJournalEntryVoucherCommand { Id = id };
                var response = await mediator.Send(command);
                return Results.Ok(response);
            })
            .WithName(nameof(SubmitJournalEntryVoucherEndpoint))
            .WithSummary("Submits a journal entry voucher for approval")
            .WithDescription("Changes JEV status from Draft to Pending for approval")
            .Produces<SubmitJournalEntryVoucherResponse>()
            .RequirePermission("Permissions.JournalEntryVouchers.Submit")
            .MapToApiVersion(1);
    }
}

