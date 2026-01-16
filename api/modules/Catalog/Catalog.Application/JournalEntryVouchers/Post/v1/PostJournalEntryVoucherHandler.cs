using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Catalog.Application.JournalEntryVouchers.Post.v1;

public sealed class PostJournalEntryVoucherHandler(
    ILogger<PostJournalEntryVoucherHandler> logger,
    [FromKeyedServices("catalog:journalentryv ouchers")] IRepository<JournalEntryVoucher> repository)
    : IRequestHandler<PostJournalEntryVoucherCommand, PostJournalEntryVoucherResponse>
{
    public async Task<PostJournalEntryVoucherResponse> Handle(PostJournalEntryVoucherCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var voucher = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new InvalidOperationException($"Journal entry voucher {request.Id} not found");

        voucher.Post();
        await repository.UpdateAsync(voucher, cancellationToken);

        logger.LogInformation("Journal entry voucher {VoucherId} posted", voucher.Id);
        return new PostJournalEntryVoucherResponse(voucher.Id);
    }
}
