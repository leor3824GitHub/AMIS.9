using AMIS.Framework.Infrastructure.Auth.Policy;
using AMIS.WebApi.Inventories.Application.JournalEntryVouchers.Get.v1;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AMIS.WebApi.Inventories.Infrastructure.Endpoints.v1.JournalEntryVoucher;

public static class GetJournalEntryVoucherEndpoint
{
    internal static RouteHandlerBuilder MapGetJournalEntryVoucherEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapGet("/{id:guid}", GetHandler)
            .WithName(nameof(GetJournalEntryVoucherEndpoint))
            .Produces<GetJournalEntryVoucherResponse>()
            .RequirePermission("Permissions.JournalEntryVouchers.View")
            .MapToApiVersion(1);
    }

    private static async Task<IResult> GetHandler(
        Guid id,
        ISender mediator,
        CancellationToken cancellationToken = default)
    {
        var command = new GetJournalEntryVoucherCommand(id);
        var response = await mediator.Send(command, cancellationToken);
        return Results.Ok(response);
    }
}

