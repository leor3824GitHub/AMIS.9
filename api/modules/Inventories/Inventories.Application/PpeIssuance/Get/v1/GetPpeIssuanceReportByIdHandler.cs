using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.PpeIssuance.Get.v1;

public sealed class GetPpeIssuanceReportByIdHandler(
    ILogger<GetPpeIssuanceReportByIdHandler> logger,
    [FromKeyedServices("inventories:ppeir")] IReadRepository<PPEIR> repository)
    : IRequestHandler<GetPpeIssuanceReportByIdQuery, GetPpeIssuanceReportByIdResponse>
{
    public async Task<GetPpeIssuanceReportByIdResponse> Handle(
        GetPpeIssuanceReportByIdQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Retrieving PPE Issuance Report with ID: {Id}", request.Id);

        var report = await repository.GetByIdAsync(request.Id, cancellationToken);

        if (report == null)
        {
            logger.LogWarning("PPE Issuance Report not found with ID: {Id}", request.Id);
            throw new KeyNotFoundException($"PPE Issuance Report with ID {request.Id} not found.");
        }

        var lineItems = report.Items.Select(li => new PpeIssuanceLineItemResponse(
            li.PropertyCode)).ToList();

        logger.LogInformation(
            "Successfully retrieved PPE Issuance Report with ID: {Id}, IR Number: {IRNumber}",
            report.Id,
            report.IRNumber);

        return new GetPpeIssuanceReportByIdResponse(
            report.Id,
            report.IRNumber,
            report.IssuedTo,
            report.Address,
            report.Type.Value,
            report.Date,
            report.Items.Count,
            (int)report.Status,
            report.Notes,
            lineItems);
    }
}

