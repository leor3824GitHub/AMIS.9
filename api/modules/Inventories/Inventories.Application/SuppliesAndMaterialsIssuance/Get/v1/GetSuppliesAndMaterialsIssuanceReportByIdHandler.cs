using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.SuppliesAndMaterialsIssuance.Get.v1;

public sealed class GetSuppliesAndMaterialsIssuanceReportByIdHandler(
    ILogger<GetSuppliesAndMaterialsIssuanceReportByIdHandler> logger,
    [FromKeyedServices("inventories:smir")] IReadRepository<SuppliesAndMaterialsIssuanceReport> readRepository)
    : IRequestHandler<GetSuppliesAndMaterialsIssuanceReportByIdQuery, GetSuppliesAndMaterialsIssuanceReportByIdResponse>
{
    public async Task<GetSuppliesAndMaterialsIssuanceReportByIdResponse> Handle(
        GetSuppliesAndMaterialsIssuanceReportByIdQuery request,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var report = await readRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"SMIR with ID {request.Id} not found");

        logger.LogInformation("Retrieved SMIR {SmirNumber} with ID {SmirId}", report.SmirNumber, report.Id);

        return new GetSuppliesAndMaterialsIssuanceReportByIdResponse(
            report.Id,
            report.SmirNumber,
            report.TransactionDate,
            report.IssuanceReason.Value,
            report.GetTotalAmount(),
            report.Notes);
    }
}

