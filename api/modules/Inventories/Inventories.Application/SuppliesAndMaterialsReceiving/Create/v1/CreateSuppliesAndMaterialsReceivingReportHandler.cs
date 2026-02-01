using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.SuppliesAndMaterialsReceiving.Create.v1;

public sealed class CreateSuppliesAndMaterialsReceivingReportHandler(
    ILogger<CreateSuppliesAndMaterialsReceivingReportHandler> logger,
    [FromKeyedServices("inventories:smrr")] IRepository<SuppliesAndMaterialsReceivingReport> repository)
    : IRequestHandler<CreateSuppliesAndMaterialsReceivingReportCommand, CreateSuppliesAndMaterialsReceivingReportResponse>
{
    public async Task<CreateSuppliesAndMaterialsReceivingReportResponse> Handle(
        CreateSuppliesAndMaterialsReceivingReportCommand request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        // Create value objects
        var source = new ReceivingSourceInfo(
            request.SourceName,
            request.SourceAddress,
            request.ReceivingDate);

        var transactionType = ReceivingTransactionType.FromString(request.TransactionType);

        var authentication = new ReceivingAuthentication(
            request.ReceivedByName,
            request.ReceivedDate,
            request.NotedByName,
            request.NotedDate);

        // Create the aggregate root
        var report = new SuppliesAndMaterialsReceivingReport(
            request.SmrrNumber,
            source,
            transactionType,
            authentication,
            request.Notes);

        // Add line items
        var lineItems = request.LineItems.Select(dto => new ReceivingLineItem(
            dto.Name,
            dto.Description,
            dto.AcquisitionDate,
            dto.Quantity,
            dto.Unit,
            dto.UnitCost,
            dto.Location,
            dto.Reference,
            dto.ClassCode,
            dto.CategoryCode,
            dto.ItemCode)).ToList();

        report.AddLineItems(lineItems);

        // Persist
        await repository.AddAsync(report, cancellationToken);

        logger.LogInformation("SMRR {SmrrNumber} created with ID {SmrrId}", report.SmrrNumber, report.Id);

        return new CreateSuppliesAndMaterialsReceivingReportResponse(report.Id);
    }
}

