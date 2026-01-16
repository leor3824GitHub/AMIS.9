using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.JournalEntryVouchers.Submit.v1;

/// <summary>
/// Handler for submitting a journal entry voucher for approval
/// </summary>
public sealed class SubmitJournalEntryVoucherHandler(
    [FromKeyedServices("catalog:journalentryvouchers")] IRepository<JournalEntryVoucher> repository)
    : IRequestHandler<SubmitJournalEntryVoucherCommand, SubmitJournalEntryVoucherResponse>
{

    public async Task<SubmitJournalEntryVoucherResponse> Handle(SubmitJournalEntryVoucherCommand request, CancellationToken cancellationToken)
    {
        var jev = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (jev == null)
        {
            throw new Exception($"Journal Entry Voucher with ID {request.Id} not found");
        }

        // Call domain method to submit for approval
        jev.Submit();
        await repository.UpdateAsync(jev, cancellationToken);

        return new SubmitJournalEntryVoucherResponse
        {
            Id = jev.Id,
            Status = jev.Status.ToString(),
            Message = "JEV submitted for approval"
        };
    }
}
