using Ardalis.Specification;
using AMIS.WebApi.Inventories.Application.JournalEntryVouchers.Search.v1;
using AMIS.WebApi.Inventories.Domain;

namespace AMIS.WebApi.Inventories.Application.JournalEntryVouchers.Search.v1;

public sealed class SearchJournalEntryVouchersSpecs : Specification<JournalEntryVoucher, JournalEntryVoucherDto>
{
    public SearchJournalEntryVouchersSpecs(SearchJournalEntryVouchersCommand request)
    {
        Query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .OrderByDescending(x => x.Created);

        Query
            .Select(v => new JournalEntryVoucherDto(
                v.Id,
                v.VoucherNumber,
                v.Year,
                v.Month,
                v.Status.ToString(),
                v.TotalDebitAmount,
                v.TotalCreditAmount,
                v.PostedDate));
    }
}

