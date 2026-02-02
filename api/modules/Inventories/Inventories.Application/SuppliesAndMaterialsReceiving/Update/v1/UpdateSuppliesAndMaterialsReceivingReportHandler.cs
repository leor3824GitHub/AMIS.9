using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shared.Authorization;

namespace AMIS.WebApi.Inventories.Application.SuppliesAndMaterialsReceiving.Update.v1;

public sealed class UpdateSuppliesAndMaterialsReceivingReportHandler(
    ILogger<UpdateSuppliesAndMaterialsReceivingReportHandler> logger,
    [FromKeyedServices("inventories:smrr")] IRepository<SuppliesAndMaterialsReceivingReport> repository,
    IAuthorizationService authorizationService)
    : IRequestHandler<UpdateSuppliesAndMaterialsReceivingReportCommand, UpdateSuppliesAndMaterialsReceivingReportResponse>
{
    public async Task<UpdateSuppliesAndMaterialsReceivingReportResponse> Handle(
        UpdateSuppliesAndMaterialsReceivingReportCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Authorization check: Only Accounting personnel with Update permission can update reports
        var authResult = await authorizationService.AuthorizeAsync(
            null,
            $"{FshResources.SuppliesAndMaterialsReceiving}.{FshActions.Update}");

        if (!authResult.Succeeded)
        {
            logger.LogWarning("Unauthorized update attempt for SMRR {Id}", request.Id);
            throw new UnauthorizedAccessException(
                "You do not have permission to update Supplies and Materials Receiving reports. Only Accounting personnel can perform this action.");
        }

        try
        {
            var report = await repository.GetByIdAsync(request.Id, cancellationToken).ConfigureAwait(false);
            if (report is null)
            {
                throw new InvalidOperationException($"SMRR with Id {request.Id} was not found.");
            }

            // Update header information
            var source = new ReceivingSourceInfo(request.SourceName, request.SourceAddress, request.SourceReceiptDate);
            var receiptType = ReceivingTransactionType.FromString(request.ReceiptType);

            // TODO: report.UpdateHeader(source, receiptType, request.Notes);

            // Clear and re-add line items
            // TODO: report.ClearLineItems();

            var lineItems = request.LineItems.Select(dto => new ReceivingLineItem(
                dto.Name,
                dto.Description,
                dto.AcquisitionDate,
                dto.Quantity,
                dto.Unit,
                dto.UnitCost,
                dto.Location ?? "Unspecified",
                reference: dto.PropertyCode,
                classCode: dto.ClassCode,
                categoryCode: dto.CategoryCode,
                itemCode: dto.ItemCode)).ToList();

            report.AddLineItems(lineItems);

            await repository.UpdateAsync(report, cancellationToken).ConfigureAwait(false);
            await repository.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            logger.LogInformation("SMRR (ID: {Id}) updated successfully by Accounting personnel.", report.Id);
            return new UpdateSuppliesAndMaterialsReceivingReportResponse(report.Id);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating SMRR {Id}.", request.Id);
            throw;
        }
    }
}
