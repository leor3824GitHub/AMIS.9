using AMIS.Framework.Core.Paging;
using MediatR;

namespace AMIS.WebApi.Inventories.Application.SemexTransactionLog.Search.v1;

public sealed record SearchSemexTransactionLogsCommand(
    int PageNumber = 1,
    int PageSize = 10,
    string? ItemCode = null,
    string? TransactionType = null,
    string? ReportNumber = null) : IRequest<PagedList<SemexTransactionLogResponse>>;
