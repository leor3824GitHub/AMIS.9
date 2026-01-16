using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Catalog.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AMIS.WebApi.Catalog.Application.JournalEntryVouchers.Search.v1;

public sealed class SearchJournalEntryVouchersHandler(
    [FromKeyedServices("catalog:journalentryvouchers")] IReadRepository<JournalEntryVoucher> repository)
    : IRequestHandler<SearchJournalEntryVouchersCommand, PagedList<JournalEntryVoucherDto>>
{
    public async Task<PagedList<JournalEntryVoucherDto>> Handle(SearchJournalEntryVouchersCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var spec = new SearchJournalEntryVouchersSpecs(request);

        var vouchers = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);
        var totalCount = await repository.CountAsync(spec, cancellationToken).ConfigureAwait(false);

        return new PagedList<JournalEntryVoucherDto>(vouchers, request.PageNumber, request.PageSize, totalCount);
    }
}
