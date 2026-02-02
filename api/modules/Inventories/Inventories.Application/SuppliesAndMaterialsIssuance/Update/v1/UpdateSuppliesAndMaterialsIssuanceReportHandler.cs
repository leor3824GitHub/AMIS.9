using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared.Authorization;

namespace AMIS.WebApi.Inventories.Application.SuppliesAndMaterialsIssuance.Update.v1;

public sealed class UpdateSuppliesAndMaterialsIssuanceReportHandler(
    ILogger<UpdateSuppliesAndMaterialsIssuanceReportHandler> logger,
    [FromKeyedServices("inventories:smir")] IRepository<SuppliesAndMaterialsIssuanceReport> repository,
    IAuthorizationService authorizationService)
    : IRequestHandler<UpdateSuppliesAndMaterialsIssuanceReportCommand, UpdateSuppliesAndMaterialsIssuanceReportResponse>
{
    public async Task<UpdateSuppliesAndMaterialsIssuanceReportResponse> Handle(
        UpdateSuppliesAndMaterialsIssuanceReportCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Authorization check: Only Accounting personnel with Update permission can update reports
        var authResult = await authorizationService.AuthorizeAsync(
            null,
            $"{FshResources.SuppliesAndMaterialsIssuance}.{FshActions.Update}");

        if (!authResult.Succeeded)
        {
            logger.LogWarning("Unauthorized update attempt for SMIR {Id}", request.Id);
            throw new UnauthorizedAccessException(
                "You do not have permission to update Supplies and Materials Issuance reports. Only Accounting personnel can perform this action.");
        }

        try
        {
            var report = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
            if (report is null)
            {
                throw new InvalidOperationException($"SMIR with Id {request.Id} was not found.");
            }

            // Update header information
            var recipient = new RecipientInfo(
                request.RecipientName,
                request.RecipientAddress,
                request.RecipientContactNumber);

            var issuanceReason = IssuanceReason.FromString(request.IssuanceReason);

            var authorization = new IssuanceAuthorization(
                request.IssuingOfficerName,
                request.IssuingDate,
                request.ApprovingOfficerName,
                request.ApprovingDate,
                request.AuthRecipientName,
                request.AuthReceiptDate,
                request.DriverName,
                request.BillOfLadingNumber);

            // TODO: report.UpdateHeader(recipient, issuanceReason, authorization, request.Notes);

            // Clear and re-add line items
            // TODO: report.ClearLineItems();

            var lineItems = request.LineItems.Select(dto => new IssuanceLineItem(
                dto.PropertyCode,
                dto.Name,
                dto.Description,
                dto.AcquisitionDate,
                dto.Quantity,
                dto.Unit,
                dto.UnitCost)).ToList();

            report.AddLineItems(lineItems);

            await repository.UpdateAsync(report, cancellationToken).ConfigureAwait(false);
            await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            logger.LogInformation("SMIR {SmirNumber} (ID: {Id}) updated successfully by Accounting personnel.", report.SmirNumber, report.Id);
            return new UpdateSuppliesAndMaterialsIssuanceReportResponse(report.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating SMIR {Id}.", request.Id);
            throw;
        }
    }
}
