using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AMIS.Framework.Core.Persistence;
using SemexTransactionLogDomain = AMIS.WebApi.Inventories.Domain.SemexTransactionLog;

namespace AMIS.WebApi.Inventories.Application.SemexTransactionLog.Get.v1;

public sealed record GetSemexTransactionLogByIdQuery(Guid Id) : IRequest<SemexTransactionLogDetailResponse>;

public sealed record SemexTransactionLogDetailResponse(
    Guid Id,
    string ItemCode,
    string TransactionType,
    string ReportNumber,
    int QuantityChange,
    int InventoryBefore,
    int InventoryAfter,
    string StatusBefore,
    string StatusAfter,
    bool Success,
    string? ErrorMessage,
    string? InitiatedBy,
    DateTime TransactionDate);

public sealed class GetSemexTransactionLogByIdHandler(
    ILogger<GetSemexTransactionLogByIdHandler> logger,
    [FromKeyedServices("inventories:semex-transaction-logs")] IReadRepository<SemexTransactionLogDomain> repository)
    : IRequestHandler<GetSemexTransactionLogByIdQuery, SemexTransactionLogDetailResponse>
{
    public async Task<SemexTransactionLogDetailResponse> Handle(
        GetSemexTransactionLogByIdQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var log = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
        if (log is null)
        {
            logger.LogWarning("Semex transaction log not found with ID {LogId}", request.Id);
            throw new InvalidOperationException($"Semex transaction log with ID {request.Id} not found.");
        }

        logger.LogInformation("Retrieved Semex transaction log {ReportNumber}", log.ReportNumber);

        return new SemexTransactionLogDetailResponse(
            log.Id,
            log.ItemCode,
            log.TransactionType,
            log.ReportNumber,
            log.QuantityChange,
            log.InventoryBefore,
            log.InventoryAfter,
            log.StatusBefore.ToString(),
            log.StatusAfter.ToString(),
            log.Success,
            log.ErrorMessage,
            log.InitiatedBy,
            log.TransactionDate);
    }
}
