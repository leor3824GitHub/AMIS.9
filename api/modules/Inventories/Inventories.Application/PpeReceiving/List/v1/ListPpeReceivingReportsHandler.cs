using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.PpeReceiving.List.v1;

public sealed class ListPpeReceivingReportsHandler(
    ILogger<ListPpeReceivingReportsHandler> logger,
    [FromKeyedServices("inventories:pperr")] IReadRepository<PpeReceivingReport> repository)
    : IRequestHandler<ListPpeReceivingReportsQuery, ListPpeReceivingReportsResponse>
{
    public async Task<ListPpeReceivingReportsResponse> Handle(
        ListPpeReceivingReportsQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving list of PPE Receiving Reports");

        var reports = await repository.ListAsync(cancellationToken);

        var reportDtos = reports
            .OrderByDescending(x => x.Created)
            .Select(x => new PpeReceivingReportDto
            {
                Id = x.Id,
                ReportNumber = x.ReportNumber,
                SourceName = x.Source.Name,
                ReceiptType = x.ReceiptType.Value,
                SourceReceiptDate = x.Source.ReceiptDate,
                Location = x.Location,
                LineItemsCount = x.LineItems.Count,
                TotalAmount = x.GetTotalAmount(),
                Status = (int)x.Status,
                CreatedAt = x.Created.DateTime,
            })
            .ToList();

        logger.LogInformation("Successfully retrieved {Count} PPE Receiving Reports", reportDtos.Count);

        return new ListPpeReceivingReportsResponse { Reports = reportDtos.AsReadOnly() };
    }
}
