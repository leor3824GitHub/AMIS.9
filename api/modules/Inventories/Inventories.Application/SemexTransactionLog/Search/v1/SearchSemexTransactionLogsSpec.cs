using Ardalis.Specification;
using SemexTransactionLogDomain = AMIS.WebApi.Inventories.Domain.SemexTransactionLog;

namespace AMIS.WebApi.Inventories.Application.SemexTransactionLog.Search.v1;

public sealed class SearchSemexTransactionLogsSpec : Specification<SemexTransactionLogDomain>
{
    public SearchSemexTransactionLogsSpec(SearchSemexTransactionLogsCommand request)
    {
        if (!string.IsNullOrWhiteSpace(request.ItemCode))
            Query.Where(l => l.ItemCode.Contains(request.ItemCode, System.StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(request.TransactionType))
            Query.Where(l => l.TransactionType.Contains(request.TransactionType, System.StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(request.ReportNumber))
            Query.Where(l => l.ReportNumber.Contains(request.ReportNumber, System.StringComparison.OrdinalIgnoreCase));

        var pageNumber = request.PageNumber <= 0 ? 1 : request.PageNumber;
        var pageSize = request.PageSize <= 0 ? 10 : request.PageSize;

        Query.OrderByDescending(l => l.TransactionDate);
        Query.Skip((pageNumber - 1) * pageSize);
        Query.Take(pageSize);
    }
}
