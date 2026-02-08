using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.PpeReceiving.Get.v1;

public sealed class GetPpeReceivingReportByIdHandler(
    ILogger<GetPpeReceivingReportByIdHandler> logger,
    [FromKeyedServices("inventories:pperr")] IReadRepository<PPERR> repository)
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

        var lineItems = report.Items
            .Select(li => new GetPpeReceivingLineItemResponse(
                li.PropertyCode,
                li.RRNumber))
            .ToList();

        return new GetPpeReceivingReportByIdResponse(
            report.Id,
            report.RRNumber,
            report.ReceivedFrom,
            report.Address,
            report.Type.Value,
            report.Date,
            report.GetTotalAmount(),
            report.Notes,
            (int)report.Status,
            lineItems);
    }
}

