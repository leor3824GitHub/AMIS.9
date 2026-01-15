using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.JournalEntryVouchers.Get.v1;

public sealed class GetJournalEntryVoucherHandler(
    ILogger<GetJournalEntryVoucherHandler> logger,
    [FromKeyedServices("catalog:journalentryv ouchers")] IReadRepository<JournalEntryVoucher> repository)
    : IRequestHandler<GetJournalEntryVoucherCommand, GetJournalEntryVoucherResponse>
{
    public async Task<GetJournalEntryVoucherResponse> Handle(GetJournalEntryVoucherCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var voucher = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new InvalidOperationException($"Journal entry voucher {request.Id} not found");

        logger.LogInformation("Retrieved journal entry voucher {VoucherId}", voucher.Id);
        return new GetJournalEntryVoucherResponse(
            voucher.Id,
            voucher.VoucherNumber,
            voucher.Year,
            voucher.Month,
            voucher.Status.ToString(),
            voucher.TotalDebitAmount,
            voucher.TotalCreditAmount,
            voucher.PostedDate);
    }
}
