using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.JournalEntryVouchers.Create.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.JournalEntryVoucher;

public static class CreateJournalEntryVoucherEndpoint
{
    internal static RouteHandlerBuilder MapJournalEntryVoucherCreationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/", async (CreateJournalEntryVoucherCommand request, ISender mediator) =>
            {
                var response = await mediator.Send(request);
                return Results.Ok(response);
            })
            .WithName(nameof(CreateJournalEntryVoucherEndpoint))
            .WithSummary("Creates a journal entry voucher")
            .WithDescription("Creates a journal entry voucher for monthly depreciation")
            .Produces<CreateJournalEntryVoucherResponse>()
            .RequirePermission("Permissions.JournalEntryVouchers.Create")
            .MapToApiVersion(1);
    }
}

