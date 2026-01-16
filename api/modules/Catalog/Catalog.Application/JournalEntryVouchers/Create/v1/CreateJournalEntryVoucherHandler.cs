using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.JournalEntryVouchers.Create.v1;

public sealed class CreateJournalEntryVoucherHandler(
    ILogger<CreateJournalEntryVoucherHandler> logger,
    [FromKeyedServices("catalog:journalentryvouchers")] IRepository<JournalEntryVoucher> repository)
    : IRequestHandler<CreateJournalEntryVoucherCommand, CreateJournalEntryVoucherResponse>
{
    public async Task<CreateJournalEntryVoucherResponse> Handle(CreateJournalEntryVoucherCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var voucher = JournalEntryVoucher.Create(request.Year, request.Month);
        await repository.AddAsync(voucher, cancellationToken);

        logger.LogInformation("Journal entry voucher created {VoucherId} for {Year}-{Month}", voucher.Id, request.Year, request.Month);
        return new CreateJournalEntryVoucherResponse(voucher.Id);
    }
}
