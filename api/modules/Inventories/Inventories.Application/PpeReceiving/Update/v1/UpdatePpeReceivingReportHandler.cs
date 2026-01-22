using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.PpeReceiving.Update.v1;

public sealed class UpdatePpeReceivingReportHandler(
    ILogger<UpdatePpeReceivingReportHandler> logger,
    [FromKeyedServices("inventories:pperr")] IRepository<PpeReceivingReport> repository)
    : IRequestHandler<UpdatePpeReceivingReportCommand, UpdatePpeReceivingReportResponse>
{
    public async Task<UpdatePpeReceivingReportResponse> Handle(UpdatePpeReceivingReportCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        try
        {
            var report = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
            if (report is null)
            {
                throw new InvalidOperationException($"PPE Receiving Report with Id {request.Id} was not found.");
            }

            var source = new PpeSourceInfo(request.SourceName, request.SourceAddress, request.SourceReceiptDate);
            var receiptType = PpeReceiptType.FromString(request.ReceiptType);

            report.UpdateHeader(source, receiptType, request.Notes);

            report.ClearLineItems();

            var lineItems = request.LineItems.Select(x => new PpeReceivingLineItem(
                x.PropertyCode,
                x.Description,
                x.DateAcquired,
                x.Quantity,
                x.Unit,
                x.UnitCost,
                x.Location)).ToList();

            report.AddLineItems(lineItems);

            await repository.UpdateAsync(report, cancellationToken).ConfigureAwait(false);
            await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            logger.LogInformation("PPE Receiving Report {Id} updated successfully.", request.Id);
            return new UpdatePpeReceivingReportResponse(report.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating PPE Receiving Report {Id}.", request.Id);
            throw;
        }
    }
}
