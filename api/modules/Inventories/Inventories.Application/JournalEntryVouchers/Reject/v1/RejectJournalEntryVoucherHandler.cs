using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Inventories.Application.JournalEntryVouchers.Reject.v1;

/// <summary>
/// Handler for rejecting a journal entry voucher
/// </summary>
public sealed class RejectJournalEntryVoucherHandler(
    [FromKeyedServices("inventories:journalentryvouchers")] IRepository<JournalEntryVoucher> repository)
    : IRequestHandler<RejectJournalEntryVoucherCommand, RejectJournalEntryVoucherResponse>
{

    public async Task<RejectJournalEntryVoucherResponse> Handle(RejectJournalEntryVoucherCommand request, CancellationToken cancellationToken)
    {
        var jev = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (jev == null)
        {
            throw new Exception($"Journal Entry Voucher with ID {request.Id} not found");
        }

        // Call domain method to reject
        jev.Reject(request.Reason ?? "");
        await repository.UpdateAsync(jev, cancellationToken);

        return new RejectJournalEntryVoucherResponse
        {
            Id = jev.Id,
            Status = jev.Status.ToString(),
            Reason = request.Reason,
            Message = "JEV rejected"
        };
    }
}

