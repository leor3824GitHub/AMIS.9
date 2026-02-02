using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using AMIS.WebApi.Inventories.Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared.Authorization;

namespace AMIS.WebApi.Inventories.Application.PpeReceiving.Update.v1;

public sealed class UpdatePpeReceivingReportHandler(
    ILogger<UpdatePpeReceivingReportHandler> logger,
    [FromKeyedServices("inventories:pperr")] IRepository<PpeReceivingReport> repository,
    IAuthorizationService authorizationService)
    : IRequestHandler<UpdatePpeReceivingReportCommand, UpdatePpeReceivingReportResponse>
{
    public async Task<UpdatePpeReceivingReportResponse> Handle(UpdatePpeReceivingReportCommand request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Authorization check: Only users with Update permission can update reports
        var authResult = await authorizationService.AuthorizeAsync(
            null,
            $"{FshResources.PpeReceiving}.{FshActions.Update}");
        
        if (!authResult.Succeeded)
        {
            logger.LogWarning("Unauthorized update attempt for PPERR {Id}", request.Id);
            throw new UnauthorizedAccessException(
                "You do not have permission to update PPE Receiving reports. Only accounting personnel can perform this action.");
        }

        try
        {
            var report = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
            if (report is null)
            {
                throw new InvalidOperationException($"PPE Receiving Report with Id {request.Id} was not found.");
            }

            // Ensure report is in Draft status before allowing updates
            if (report.Status == PpeReportStatus.Cancelled)
            {
                logger.LogWarning("Update attempt for cancelled PPERR {Id}", request.Id);
                throw new InvalidOperationException(
                    $"PPE Receiving Report with Id {request.Id} cannot be updated. Reports in Cancelled status cannot be modified.");
            }

            // If the report is Posted, only Accounting personnel can update it for data integrity
            // Supply officers can update Draft reports, but Posted reports are protected
            if (report.Status == PpeReportStatus.Posted)
            {
                logger.LogWarning("Posted PPERR {Id} update attempt - only accounting personnel can update posted reports", request.Id);
                throw new InvalidOperationException(
                    $"PPE Receiving Report with Id {request.Id} is already Posted. Only Accounting personnel can modify posted reports for data integrity purposes. If changes are necessary, please contact your Accounting department.");
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
                x.Location,
                x.ClassCode,
                x.CategoryCode,
                x.ItemCode)).ToList();

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
