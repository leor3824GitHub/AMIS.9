using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.PpeIssuance.Create.v1;

public sealed class CreatePpeIssuanceReportHandler(
    ILogger<CreatePpeIssuanceReportHandler> logger,
    [FromKeyedServices("inventories:ppeir")] IRepository<PpeIssuanceReport> issuanceRepository)
    : IRequestHandler<CreatePpeIssuanceReportCommand, CreatePpeIssuanceReportResponse>
{
    public async Task<CreatePpeIssuanceReportResponse> Handle(
        CreatePpeIssuanceReportCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation(
                "Creating new PPE Issuance Report with report number: {ReportNumber} for recipient: {RecipientName}",
                request.ReportNumber,
                request.RecipientName);

            var recipientInfo = new PpeRecipientInfo(
                request.RecipientName,
                request.RecipientAddress);

            var issuanceType = PpeIssuanceType.FromString(request.IssuanceType);

            var report = new PpeIssuanceReport(
                request.ReportNumber,
                recipientInfo,
                issuanceType,
                request.IssuanceDate,
                request.Notes);

            foreach (var lineItem in request.LineItems)
            {
                var item = new PpeIssuanceLineItem(
                    lineItem.PropertyCode,
                    null,
                    lineItem.Description,
                    DateTime.UtcNow,
                    lineItem.AcquisitionCost,
                    lineItem.AccumulatedDepreciation,
                    lineItem.BookValue,
                    lineItem.Location);
                report.AddLineItem(item);
            }

            await issuanceRepository.AddAsync(report, cancellationToken);
            await issuanceRepository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            logger.LogInformation(
                "Successfully created PPE Issuance Report as Draft with ID: {Id} and report number: {ReportNumber}",
                report.Id,
                report.ReportNumber);

            return new CreatePpeIssuanceReportResponse(
                report.Id,
                report.ReportNumber,
                report.Created.DateTime);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Error creating PPE Issuance Report with report number: {ReportNumber}",
                request.ReportNumber);
            throw;
        }
    }
}

