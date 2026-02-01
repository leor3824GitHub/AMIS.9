using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.SuppliesAndMaterialsIssuance.Create.v1;

public sealed class CreateSuppliesAndMaterialsIssuanceReportHandler(
    ILogger<CreateSuppliesAndMaterialsIssuanceReportHandler> logger,
    [FromKeyedServices("inventories:smir")] IRepository<SuppliesAndMaterialsIssuanceReport> repository)
    : IRequestHandler<CreateSuppliesAndMaterialsIssuanceReportCommand, CreateSuppliesAndMaterialsIssuanceReportResponse>
{
    public async Task<CreateSuppliesAndMaterialsIssuanceReportResponse> Handle(
        CreateSuppliesAndMaterialsIssuanceReportCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Create value objects
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

        // Create the aggregate root
        var report = new SuppliesAndMaterialsIssuanceReport(
            request.SmirNumber,
            request.TransactionDate,
            recipient,
            issuanceReason,
            authorization,
            request.Notes);

        // Add line items
        var lineItems = request.LineItems.Select(dto => new IssuanceLineItem(
            dto.PropertyCode,
            dto.Name,
            dto.Description,
            dto.AcquisitionDate,
            dto.Quantity,
            dto.Unit,
            dto.UnitCost)).ToList();

        report.AddLineItems(lineItems);

        // Persist
        await repository.AddAsync(report, cancellationToken);

        logger.LogInformation("SMIR {SmirNumber} created with ID {SmirId}", report.SmirNumber, report.Id);

        return new CreateSuppliesAndMaterialsIssuanceReportResponse(report.Id);
    }
}

