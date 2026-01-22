using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.PpeReceiving.Create.v1;

public sealed class CreatePpeReceivingReportHandler(
    ILogger<CreatePpeReceivingReportHandler> logger,
    [FromKeyedServices("inventories:pperr")] IRepository<PpeReceivingReport> receivingRepository)
    : IRequestHandler<CreatePpeReceivingReportCommand, CreatePpeReceivingReportResponse>
{
    public async Task<CreatePpeReceivingReportResponse> Handle(CreatePpeReceivingReportCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        try
        {
            var source = new PpeSourceInfo(request.SourceName, request.SourceAddress, request.SourceReceiptDate);
            var receiptType = PpeReceiptType.FromString(request.ReceiptType);

            var report = new PpeReceivingReport(
                request.ReportNumber,
                source,
                receiptType,
                request.Notes);

            var lineItems = request.LineItems.Select(x => new PpeReceivingLineItem(
                x.PropertyCode,
                x.Description,
                x.DateAcquired,
                x.Quantity,
                x.Unit,
                x.UnitCost,
                x.Location)).ToList();

            report.AddLineItems(lineItems);

            await receivingRepository.AddAsync(report, cancellationToken);
            await receivingRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            logger.LogInformation("PPE Receiving Report {ReportNumber} created as Draft.", request.ReportNumber);
            return new CreatePpeReceivingReportResponse(report.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating PPE Receiving Report.");
            throw;
        }
    }
}

