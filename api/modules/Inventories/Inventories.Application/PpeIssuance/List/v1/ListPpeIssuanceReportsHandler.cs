using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.PpeIssuance.List.v1;

public sealed class ListPpeIssuanceReportsHandler(
    ILogger<ListPpeIssuanceReportsHandler> logger,
    [FromKeyedServices("inventories:ppeir")] IReadRepository<PpeIssuanceReport> repository)
    : IRequestHandler<ListPpeIssuanceReportsQuery, ListPpeIssuanceReportsResponse>
{
    public async Task<ListPpeIssuanceReportsResponse> Handle(
        ListPpeIssuanceReportsQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving list of PPE Issuance Reports");

        var reports = await repository.ListAsync(cancellationToken);

        var reportDtos = reports
            .OrderByDescending(x => x.Created)
            .Select(x => new PpeIssuanceReportDto
            {
                Id = x.Id,
                ReportNumber = x.ReportNumber,
                RecipientName = x.Recipient.Name,
                IssuanceType = x.IssuanceType.Value,
                IssuanceDate = x.IssuanceDate,
                LineItemsCount = x.LineItems.Count,
                CreatedAt = x.Created.DateTime,
            })
            .ToList();

        logger.LogInformation("Successfully retrieved {Count} PPE Issuance Reports", reportDtos.Count);

        return new ListPpeIssuanceReportsResponse { Reports = reportDtos.AsReadOnly() };
    }
}
