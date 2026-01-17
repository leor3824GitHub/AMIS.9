using AMIS.Framework.Core.Persistence;
using AMIS.WebApi.Inventories.Domain;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AMIS.WebApi.Inventories.Application.PpeIssuance.Get.v1;

public sealed class GetPpeIssuanceReportByIdHandler(
    ILogger<GetPpeIssuanceReportByIdHandler> logger,
    [FromKeyedServices("inventories:ppeir")] IReadRepository<PpeIssuanceReport> repository)
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

        var totalCost = report.GetTotalAcquisitionCost();

        logger.LogInformation(
            "Successfully retrieved PPE Issuance Report with ID: {Id}, Report Number: {ReportNumber}",
            report.Id,
            report.ReportNumber);

        return new GetPpeIssuanceReportByIdResponse(
            report.Id,
            report.ReportNumber,
            report.Recipient.Name,
            report.Recipient.Address,
            report.IssuanceType.Value,
            report.IssuanceDate,
            totalCost,
            report.LineItems.Count,
            report.Created.DateTime);
    }
}

