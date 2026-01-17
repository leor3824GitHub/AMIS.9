using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.PpeReceiving.Get.v1;

public sealed class GetPpeReceivingReportByIdHandler(
    ILogger<GetPpeReceivingReportByIdHandler> logger,
    [FromKeyedServices("inventories:pperr")] IReadRepository<PpeReceivingReport> repository)
    : IRequestHandler<GetPpeReceivingReportByIdQuery, GetPpeReceivingReportByIdResponse>
{
    public async Task<GetPpeReceivingReportByIdResponse> Handle(GetPpeReceivingReportByIdQuery request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);

        var report = await repository.GetByIdAsync(request.Id, cancellationToken);
        if (report is null)
        {
            logger.LogWarning("PPE Receiving Report with ID {Id} not found.", request.Id);
            throw new KeyNotFoundException($"PPE Receiving Report with ID {request.Id} not found.");
        }

        return new GetPpeReceivingReportByIdResponse(
            report.Id,
            report.ReportNumber,
            report.Location,
            report.Source.Name,
            report.ReceiptType.Value,
            report.GetTotalAmount(),
            report.Notes);
    }
}

