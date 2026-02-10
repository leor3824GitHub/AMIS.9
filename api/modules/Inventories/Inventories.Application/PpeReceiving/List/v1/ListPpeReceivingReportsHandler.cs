using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.PpeReceiving.List.v1;

public sealed class ListPpeReceivingReportsHandler(
    ILogger<ListPpeReceivingReportsHandler> logger,
    [FromKeyedServices("inventories:pperr")] IReadRepository<PPERR> repository)
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
                RRNumber = x.RRNumber,
                ReceivedFrom = x.ReceivedFrom,
                Address = x.Address,
                Type = x.Type.Value,
                Date = x.Date,
                ItemsCount = x.Items.Count,
                TotalAmount = x.GetTotalAmount(),
                Status = (int)x.Status,
                CreatedAt = x.Created.DateTime,
            })
            .ToList();

        logger.LogInformation("Successfully retrieved {Count} PPE Receiving Reports", reportDtos.Count);

        return new ListPpeReceivingReportsResponse { Reports = reportDtos.AsReadOnly() };
    }
}
