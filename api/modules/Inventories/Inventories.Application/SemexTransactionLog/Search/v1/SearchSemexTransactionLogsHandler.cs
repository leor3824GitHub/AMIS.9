using AMIS.Framework.Core.Paging;
using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SemexTransactionLogDomain = AMIS.WebApi.Inventories.Domain.SemexTransactionLog;

namespace AMIS.WebApi.Inventories.Application.SemexTransactionLog.Search.v1;

public sealed class SearchSemexTransactionLogsHandler(
    ILogger<SearchSemexTransactionLogsHandler> logger,
    [FromKeyedServices("inventories:semex-transaction-logs")] IReadRepository<SemexTransactionLogDomain> repository)
    : IRequestHandler<SearchSemexTransactionLogsCommand, PagedList<SemexTransactionLogResponse>>
{
    public async Task<PagedList<SemexTransactionLogResponse>> Handle(
        SearchSemexTransactionLogsCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        logger.LogInformation("Searching Semex transaction logs with filters - ItemCode: {ItemCode}, TransactionType: {TransactionType}, ReportNumber: {ReportNumber}, Page: {PageNumber}",
            request.ItemCode, request.TransactionType, request.ReportNumber, request.PageNumber);

        var spec = new SearchSemexTransactionLogsSpec(request);
        var items = await repository.ListAsync(spec, cancellationToken).ConfigureAwait(false);

        // Get total count without pagination for PagedList
        var countSpec = new SearchSemexTransactionLogsSpecForCount(request);
        var totalCount = await repository.CountAsync(countSpec, cancellationToken).ConfigureAwait(false);

        var list = items.Select(l => new SemexTransactionLogResponse(
                l.Id,
                l.ItemCode,
                l.TransactionType,
                l.ReportNumber,
                l.QuantityChange,
                l.InventoryBefore,
                l.InventoryAfter,
                l.StatusBefore.ToString(),
                l.StatusAfter.ToString(),
                l.Success,
                l.ErrorMessage,
                l.InitiatedBy,
                l.TransactionDate))
            .ToList();

        logger.LogInformation("Found {Count} Semex transaction log entries", totalCount);

        return new PagedList<SemexTransactionLogResponse>(
            list,
            request.PageNumber,
            request.PageSize,
            totalCount);
    }
}
