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
    [FromKeyedServices("inventories:ppeir")] IRepository<PPEIR> repository,
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

            // Ensure report is in Draft status before allowing updates
            if (report.Status != PpeReportStatus.Draft)
            {
                logger.LogWarning("Update attempt for non-Draft PPEIR {Id} with status {Status}", request.Id, report.Status);
                throw new InvalidOperationException(
                    $"PPE Issuance Report with Id {request.Id} cannot be updated. Only Draft reports can be modified.");
            }

            report.UpdateHeader(
                request.IssuedTo,
                request.Address,
                PpeIssueType.FromString(request.Type),
                request.Date,
                request.Notes);

            report.ClearItems();

            foreach (var lineItem in request.LineItems)
            {
                var item = new PPEIRLineItem(
                    lineItem.PropertyCode,
                    report.IRNumber);
                report.AddItem(item);
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
