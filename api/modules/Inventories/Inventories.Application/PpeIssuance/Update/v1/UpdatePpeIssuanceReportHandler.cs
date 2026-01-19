using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.PpeIssuance.Update.v1;

public sealed class UpdatePpeIssuanceReportHandler(
    ILogger<UpdatePpeIssuanceReportHandler> logger,
    [FromKeyedServices("inventories:ppeir")] IRepository<PpeIssuanceReport> repository)
    : IRequestHandler<UpdatePpeIssuanceReportCommand, UpdatePpeIssuanceReportResponse>
{
    public async Task<UpdatePpeIssuanceReportResponse> Handle(UpdatePpeIssuanceReportCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        try
        {
            var report = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
            if (report is null)
            {
                throw new InvalidOperationException($"PPE Issuance Report with Id {request.Id} was not found.");
            }

            var recipientInfo = new PpeRecipientInfo(request.RecipientName, request.RecipientAddress);
            var issuanceType = PpeIssuanceType.FromString(request.IssuanceType);

            report.UpdateHeader(recipientInfo, issuanceType, request.IssuanceDate, request.Notes);

            report.ClearLineItems();

            foreach (var lineItem in request.LineItems)
            {
                var item = new PpeIssuanceLineItem(
                    lineItem.PropertyCode,
                    null,
                    lineItem.Description,
                    lineItem.DateAcquired ?? DateTime.UtcNow,
                    lineItem.AcquisitionCost,
                    lineItem.AccumulatedDepreciation,
                    lineItem.BookValue);
                report.AddLineItem(item);
            }

            await repository.UpdateAsync(report, cancellationToken).ConfigureAwait(false);
            await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            logger.LogInformation("PPE Issuance Report {Id} updated successfully.", request.Id);
            return new UpdatePpeIssuanceReportResponse(report.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating PPE Issuance Report {Id}.", request.Id);
            throw;
        }
    }
}
