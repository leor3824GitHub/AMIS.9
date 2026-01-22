using Ardalis.Specification;
using SemexTransactionLogDomain = AMIS.WebApi.Inventories.Domain.SemexTransactionLog;

namespace AMIS.WebApi.Inventories.Application.SemexTransactionLog.Search.v1;

public sealed class SearchSemexTransactionLogsSpecForCount : Specification<SemexTransactionLogDomain>
{
    public SearchSemexTransactionLogsSpecForCount(SearchSemexTransactionLogsCommand request)
    {
        if (!string.IsNullOrWhiteSpace(request.ItemCode))
            Query.Where(l => l.ItemCode.Contains(request.ItemCode, System.StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(request.TransactionType))
            Query.Where(l => l.TransactionType.Contains(request.TransactionType, System.StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(request.ReportNumber))
            Query.Where(l => l.ReportNumber.Contains(request.ReportNumber, System.StringComparison.OrdinalIgnoreCase));
    }
}
