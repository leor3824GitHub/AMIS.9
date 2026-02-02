using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared.Authorization;

namespace AMIS.WebApi.Inventories.Application.PpeIssuance.Update.v1;

public sealed class UpdatePpeIssuanceReportHandler(
    ILogger<UpdatePpeIssuanceReportHandler> logger,
    [FromKeyedServices("inventories:ppeir")] IRepository<PpeIssuanceReport> repository,
    IAuthorizationService authorizationService)
    : IRequestHandler<UpdatePpeIssuanceReportCommand, UpdatePpeIssuanceReportResponse>
{
    public async Task<UpdatePpeIssuanceReportResponse> Handle(UpdatePpeIssuanceReportCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Authorization check: Only users with Update permission can update reports
        var authResult = await authorizationService.AuthorizeAsync(
            null,
            $"{FshResources.PpeIssuance}.{FshActions.Update}");
        
        if (!authResult.Succeeded)
        {
            logger.LogWarning("Unauthorized update attempt for PPEIR {Id}", request.Id);
            throw new UnauthorizedAccessException(
                "You do not have permission to update PPE Issuance reports. Only accounting personnel can perform this action.");
        }

        try
        {
            var report = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
            if (report is null)
            {
                throw new InvalidOperationException($"PPE Issuance Report with Id {request.Id} was not found.");
            }

            // Ensure report is in Draft or Posted status before allowing updates
            if (report.Status == PpeReportStatus.Cancelled)
            {
                logger.LogWarning("Update attempt for cancelled PPEIR {Id}", request.Id);
                throw new InvalidOperationException(
                    $"PPE Issuance Report with Id {request.Id} cannot be updated. Reports in Cancelled status cannot be modified.");
            }

            // If the report is Posted, only Accounting personnel can update it for data integrity
            // Supply officers can update Draft reports, but Posted reports are protected
            if (report.Status == PpeReportStatus.Posted)
            {
                logger.LogWarning("Posted PPEIR {Id} update attempt - only accounting personnel can update posted reports", request.Id);
                throw new InvalidOperationException(
                    $"PPE Issuance Report with Id {request.Id} is already Posted. Only Accounting personnel can modify posted reports for data integrity purposes. If changes are necessary, please contact your Accounting department.");
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
                    lineItem.BookValue,
                    lineItem.Location);
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
