using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.SuppliesAndMaterialsReceiving.Get.v1;

public sealed class GetSuppliesAndMaterialsReceivingReportByIdHandler(
    ILogger<GetSuppliesAndMaterialsReceivingReportByIdHandler> logger,
    [FromKeyedServices("inventories:smrr")] IReadRepository<SuppliesAndMaterialsReceivingReport> readRepository)
    : IRequestHandler<GetSuppliesAndMaterialsReceivingReportByIdQuery, GetSuppliesAndMaterialsReceivingReportByIdResponse>
{
    public async Task<GetSuppliesAndMaterialsReceivingReportByIdResponse> Handle(
        GetSuppliesAndMaterialsReceivingReportByIdQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var report = await readRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"SMRR with ID {request.Id} not found");

        logger.LogInformation("Retrieved SMRR {SmrrNumber} with ID {SmrrId}", report.SmrrNumber, report.Id);

        return new GetSuppliesAndMaterialsReceivingReportByIdResponse(
            report.Id,
            report.SmrrNumber,
            report.Location,
            report.TransactionType.Value,
            report.GetTotalAmount(),
            report.Notes);
    }
}

