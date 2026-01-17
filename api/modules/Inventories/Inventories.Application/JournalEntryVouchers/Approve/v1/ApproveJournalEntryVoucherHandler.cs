using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Inventories.Application.JournalEntryVouchers.Approve.v1;

/// <summary>
/// Handler for approving a journal entry voucher
/// </summary>
public sealed class ApproveJournalEntryVoucherHandler(
    [FromKeyedServices("inventories:journalentryvouchers")] IRepository<JournalEntryVoucher> repository)
    : IRequestHandler<ApproveJournalEntryVoucherCommand, ApproveJournalEntryVoucherResponse>
{

    public async Task<ApproveJournalEntryVoucherResponse> Handle(ApproveJournalEntryVoucherCommand request, CancellationToken cancellationToken)
    {
        var jev = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (jev == null)
        {
            throw new Exception($"Journal Entry Voucher with ID {request.Id} not found");
        }

        // Call domain method to approve
        jev.Approve();
        await repository.UpdateAsync(jev, cancellationToken);

        return new ApproveJournalEntryVoucherResponse
        {
            Id = jev.Id,
            Status = jev.Status.ToString(),
            Message = "JEV approved and posted to ledger"
        };
    }
}

